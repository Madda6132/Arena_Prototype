using UnityEngine;
using RPG.Creatures;
using RPG.Inventory;

// Currently imported from my other project so it needs fixing
// Other project "Test#Open Legend"

public class EnemyManager : MonoBehaviour, ICreatureControler /*, ISaveable, IObserverDeath*/
{
    Creature creature;
    EnemyLocomotionManager enemyLocomotionManager;
    bool isManagerActive = true;
    public bool isPreformingAction = false;

    Equipment weaponEquipment;
    Equipment rightWeapon;

    //public EnemyAttackAction[] enemyAttackActions;
    //public EnemyAttackAction currentAttack;
    //[SerializeField] AbilityScriptObject ability;
    //public EquipmentSlotManager equipmentSlotManager;

    //PlayerAttack playerAttack;
    [SerializeField] Transform eyePosition;
    //[SerializeField] PatrolPath patrolPath;

    [Header("A.I Settings")]
    public float detectionRadius = 20;
    //DetectionAngle is the angle infront of the creature min is left of the creature and max is the right of the creature
    public float minimumDetectionAngle = -50f;
    public float maximumDetectionAngle = 50f;
    public float intresstTimer = 3f;
    public LayerMask detectionLayer;

    [SerializeField] float rotationSpeed = 2;
    float timer = Mathf.Infinity;
    //float attackTimer = Mathf.Infinity;
    State currentState = State.Pattrol; 
    public Transform currentTarget {private set; get;}
    Vector3 guardPosition;
    int waypointIndex = 0;

     
    private void OnDrawGizmosSelected()
    {

        Quaternion upRayRotation = Quaternion.AngleAxis(minimumDetectionAngle, Vector3.up);
        Quaternion downRayRotation = Quaternion.AngleAxis(maximumDetectionAngle, Vector3.up);

        Vector3 upRayDirection = upRayRotation * transform.forward * detectionRadius;
        Vector3 downRayDirection = downRayRotation * transform.forward * detectionRadius;

        Gizmos.DrawRay(transform.position + Vector3.up, upRayDirection);
        Gizmos.DrawRay(transform.position + Vector3.up, downRayDirection);
        Gizmos.DrawLine(transform.position + Vector3.up + downRayDirection, transform.position + Vector3.up + upRayDirection);
    }


    private void Awake()
    {
        enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        creature = GetComponent<Creature>();
        weaponEquipment = creature.EquipmentManager.GetWeaponEquipment; 
        //equipmentSlotManager = GetComponent<EquipmentSlotManager>();
        //if (patrolPath != null) { guardPosition = patrolPath.GetWaypoint(waypointIndex); 
        //} else {
        //    guardPosition = transform.position;
        //}
        //playerAttack = GetComponent<PlayerAttack>();
        //GetComponent<CreatureController>().SubToCreatureDeath(this);
    }
     
    // Update is called once per frame
    void Update()
    {
        if (!isManagerActive) return;

        HandleDetection();
        HandleCurrentAction();

        timer += Time.deltaTime;
        //attackTimer += Time.deltaTime;
    }
    private void HandleCurrentAction()
    {
         
        switch (currentState)
        {
            case State.Pattrol:
                //PattrolBehaviour();
                break;

            case State.Inspect:
                if (timer >= intresstTimer) ChangeState(State.Pattrol);
                currentTarget = null;
                //playerAttack.SetTarget(currentTarget);
                //Wait before going back to patroll
                break;

            case State.Attack:
                AttackBehaviour();

                break;
            default:
                break;
        }

        
    }

    //private void PattrolBehaviour()
    //{

    //    if(patrolPath != null)
    //    {
    //        if (DistanceToTarget(guardPosition) <= 1f)
    //        {
    //            waypointIndex = patrolPath.GetNextIndex(waypointIndex);
    //            guardPosition = patrolPath.GetWaypoint(waypointIndex);

    //        }
    //    }

    //    enemyLocomotionManager.HandleMoveToTarget(guardPosition, EnemyLocomotionManager.TravleSpeed.Walk);
    //}

    private void AttackBehaviour()
    {
        //Check if dead or board
        if (!currentTarget.GetComponent<Creature>().isAlive || timer >= intresstTimer) {
            ChangeState(State.Inspect);
            return;
        }


        float EquipedWeapon = 2f;

        if (DistanceToTarget(currentTarget.transform.position) <= EquipedWeapon) {  //equipmentSlotManager.currentlyEquipedWeapon.Range) {
            RaycastHit hit;

            Debug.DrawRay(eyePosition.position, ((currentTarget.transform.position + (Vector3.up * currentTarget.GetComponent<Collider>().bounds.max.y * 0.75f)) - eyePosition.position), Color.blue, 2f);
            if (Physics.Raycast(eyePosition.position, ((currentTarget.transform.position + (Vector3.up * currentTarget.GetComponent<Collider>().bounds.max.y * 0.75f)) - eyePosition.position), out hit,
            //Mathf.RoundToInt(equipmentSlotManager.currentlyEquipedWeapon.Range + 0.5f)) && hit.collider.tag != "Player") {
                Mathf.RoundToInt(EquipedWeapon + 0.5f)) && hit.collider.tag != "Player") {
                    enemyLocomotionManager.HandleMoveToTarget(currentTarget.transform.position);
                return;
            }

            enemyLocomotionManager.ActivateMovement(false);
            AttackTarget();

        } else {

            enemyLocomotionManager.HandleMoveToTarget(currentTarget.transform.position);
        }

    }

    private void ChangeState(State state)
    {
        currentState = state;
        timer = 0;
    }

    private float DistanceToTarget(Vector3 pos)
    {
        return Vector3.Distance(pos, transform.position);
    }

    private void AttackTarget()
    {
        Quaternion creatureRot = transform.rotation;
        Quaternion lookAtRotation = Quaternion.RotateTowards(transform.rotation,
            Quaternion.LookRotation((currentTarget.transform.position - transform.position).normalized), rotationSpeed * Time.deltaTime);

        Quaternion newRotation = new Quaternion(creatureRot.x, lookAtRotation.y, creatureRot.z, creatureRot.w);
        transform.rotation = newRotation;


        //ability.Use(gameObject, true); 

    }

    public void HandleDetection()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].tag == "Player")
            {
                Transform targetTransform = colliders[i].GetComponent<Transform>();
                
                //Check if creature is facing target
                Vector3 targetDirection = targetTransform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if ((viewableAngle > minimumDetectionAngle && viewableAngle < maximumDetectionAngle) || currentState == State.Attack)
                {

                    float dis = Vector3.Distance(transform.position + Vector3.up, targetTransform.position + Vector3.up);
                    //Check if a object is in the way
                    RaycastHit hit;
                    Debug.DrawRay(eyePosition.position, ((targetTransform.position + (Vector3.up * colliders[i].bounds.max.y * 0.75f)) - eyePosition.position) , Color.green, 2f);
                    if (Physics.Raycast(eyePosition.position, ((targetTransform.position + (Vector3.up * colliders[i].bounds.max.y * 0.75f)) - eyePosition.position), out hit, dis) && 
                        hit.collider.tag != "Player") 
                        return;

                    if (!targetTransform.GetComponent<Creature>().isAlive) return;

                     currentTarget = targetTransform;
                    //playerAttack.SetTarget(currentTarget);
                    ChangeState(State.Attack);
                }

            }
        }
    }

    public void DisabledControler() => isManagerActive = false;

    public void EnableControler() => isManagerActive = true;
    

    //public object CaptureState()
    //{

    //    return new SaveState(guardPosition);
    //}

    //public void RestoreState(object state)
    //{
    //    guardPosition = ((SaveState)state).GetVector();
    //}

    //public void uponDeath(Creature sender)
    //{
    //    deactivateManager = true;
    //    enemyLocomotionManager.ActivateMovement(false);
    //}

    [System.Serializable]
    private class SaveState {

        float x = 0;
        float y = 0;
        float z = 0;
        public SaveState(Vector3 pos)
        {
            this.x = pos.x;
            this.y = pos.y;
            this.z = pos.z;
        }

        public Vector3 GetVector()
        {
            return new Vector3(x, y, z);
        }
    }

    enum State
    {
        Pattrol,
        Inspect,
        Attack
    }

}
