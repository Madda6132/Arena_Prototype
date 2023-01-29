using UnityEngine;
using UnityEngine.AI;
using RPG.Creatures;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyLocomotionManager : MonoBehaviour
{
    
    NavMeshAgent navMeshAgent;
    AnimatorHandler animatorManager;

    [Header("A.I behavior")]
    public float stoppingDistnace = 2; 

    bool isInteractiong = false;

    float navMeshAgentStartSpeed = 6;

    private void Awake()
    {
        
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgentStartSpeed = navMeshAgent.speed;
    }

    private void Start() {

        animatorManager = GetComponent<Creature>().ActionHandler.AnimatorHandler;
    }

    private void Update()
    {
        
        UpdateAnimation();
        CheckInteraction();
    }

    //Check to see if AI is busy with interaction to prevent other actions
    private void CheckInteraction()
    {
        isInteractiong = animatorManager.GetAnimatorBool("IsInteracting");
    }

    public void HandleMoveToTarget(Vector3 pos, TravleSpeed travleSpeed = TravleSpeed.Run)
    {
        
        if (isInteractiong) return;
        switch (travleSpeed)
        {
            case TravleSpeed.Walk:
                navMeshAgent.speed = navMeshAgentStartSpeed * 0.2f;
                break;

            default:
            case TravleSpeed.Run:
                navMeshAgent.speed = navMeshAgentStartSpeed;
                break;

        }

        ActivateMovement(true);
        navMeshAgent.SetDestination(pos);
        
    }

    public void ActivateMovement(bool AllowMovement)
    {
        navMeshAgent.isStopped = !AllowMovement;
    }
    

    private void UpdateAnimation()
    {
        Vector3 velocity = navMeshAgent.velocity;
        if (navMeshAgentStartSpeed != 0) velocity /= navMeshAgentStartSpeed;
         Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        
        animatorManager.UpdateAnimatorMovementValues(0, localVelocity.z, false);
    }


    public enum TravleSpeed
    {
        Walk,
        Run
    }

}
