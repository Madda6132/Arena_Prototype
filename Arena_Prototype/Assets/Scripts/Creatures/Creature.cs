using UnityEngine;
using RPG.Statistics;
using System;
using RPG.Combat;

namespace RPG.Creatures {
    /// <summary>
    /// A Avatar with stats that can be used for player or AI
    /// </summary>
    public class Creature : MonoBehaviour, IDamageable {

        //Combat target point (For example projectiles fly towards this)
        public Transform TargetMark { get => creatureTargetMark; set => creatureTargetMark = value; }

        [Header("Other")]
        ///A gameObject that determines the creatures direction
        [SerializeField] Transform creaturelookDirectionObject;
        [SerializeField] Transform creatureTargetMark;

        //Plans in the future will be extracted to a ScriptableObject
        [Header("Stats")]
        [SerializeField] StatHealth health;
        [SerializeField] StatStrength strength;
        [SerializeField] StatDexterity dexterity;
        [SerializeField] StatIntelligence intelligence;


        readonly IStatisticsController statistic = new StatisticLogic();

        //Controller that controls the creature (Player/AI)
        ICreatureControler creatureControler;

        HealthManager healthManager;
        public AfflictionManager AfflictionManager { get; private set; }
        public EquipmentManager EquipmentManager { get; private set; }
        public Actions.PerformActionHandler ActionHandler { get; private set; }

        public bool isAlive { get; private set; } = true;
        public bool incapacitated { get; private set; } = false;

        public Action<int> OnTakeHit;

        private void Awake() {

            //Stats
            SetStatistics();

            healthManager = new(gameObject, statistic);
            AfflictionManager = new();
            ActionHandler = new(this);
            EquipmentManager = new(this);

            creatureControler = GetComponent<ICreatureControler>();

            OnTakeHit += TakeDamage;
            OnTakeHit += (int damage) => ActionHandler.AnimatorHandler.PlayTargetAnimation("Impact", 2);

            //Transforms
            TargetMark = creatureTargetMark;
        }

        //Needs to handle what happens in the effects
        public void TakeCombatEffect(EffectInformation effectInformation) {
            throw new NotImplementedException();
        }

        public void TakeDamage(int damage) {

            //---TODO---
            //Perform hurt animation

            if (healthManager.TakeDamage(damage)) Death();
        }

        /// <summary>
        /// PLayer => Camera / AI => Target or forward
        /// </summary>
        public (Vector3, Ray, RaycastHit) GetRayLook() {

            Ray ray = new(creaturelookDirectionObject.position, creaturelookDirectionObject.forward);
            
            Physics.Raycast(ray, out RaycastHit hitInfo, 30f); 
            return (creaturelookDirectionObject.up, ray, hitInfo);
        }
        public IStatisticsController GetStatistics => statistic;
        public void SubToHealth(Action<int, int> action) => healthManager.AddHealthListener(action);
        public void UnsubToHealth(Action<int, int> action) => healthManager.RemoveHealthListener(action);

        //Animator sends messages to this
        public void AnimationReciver(string animationTrigger) => ActionHandler.AnimationReciver(animationTrigger);

        private void Death() {

            creatureControler.DisabledControler();
            isAlive = false;
            //Dead cause death and "Stun" target
        }

        private void SetStatistics() {

            statistic.CreateStat(health);
            statistic.CreateStat(strength);
            statistic.CreateStat(dexterity);
            statistic.CreateStat(intelligence);
        }

        
    }

}
