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
        public ICreatureControler creatureControler { get; private set; }

        HealthManager healthManager;
        public AfflictionManager AfflictionManager { get; private set; }
        public EquipmentManager EquipmentManager { get; private set; }
        public Actions.PerformActionHandler ActionHandler { get; private set; }

        public bool IsAlive {
            get {
                return _IsAlive;
            }
            private set {
                _IsAlive = value;
                IsAliveObserver.OnResultUpdate(_IsAlive);
            }
        }
        public bool IsIncapacitated {
            get {

                return _IsIncapacitated;
            } 
            private set {

                _IsIncapacitated = value;
                IsIncapacitatedObserver.OnResultUpdate(_IsIncapacitated);
            }
        }

        public ResultObserver<bool> IsAliveObserver = new();
        public ResultObserver<bool> IsIncapacitatedObserver = new();

        /// <summary>
        /// Calls out when the creature takes a hit
        /// </summary>
        public ResultObserver<int> OnImpactObserver = new();

        private bool _IsAlive = true;
        private bool _IsIncapacitated = false;

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
        /// <summary>
        /// Animation Events calls this method and separates index from message by '-'and removes empty spaces
        /// </summary>
        /// <param name="message_Index">message with attached layer index</param>
        public void AnimationMessageReciver(string message_Index) {

            int layerIndex = 0;
            string message = "";

            string[] splitMessage = message_Index.Split('-', StringSplitOptions.RemoveEmptyEntries);

            
            if(!int.TryParse(splitMessage[1], out layerIndex)) {

                Debug.LogError("Could not correctly extract layer index from " + message_Index + " with -");
                return;
            }
            
            message = splitMessage[0];
            ActionHandler.AnimationMessageReciver(layerIndex, message);
        }
        public void AnimationMessageReciver(int layerIndex, string message) => ActionHandler.AnimationMessageReciver(layerIndex, message);

        /*---Private---*/

        private void Awake() {

            //Stats
            SetStatistics();

            healthManager = new(gameObject, statistic);
            AfflictionManager = new();
            ActionHandler = new(this);
            EquipmentManager = new(this);

            creatureControler = GetComponent<ICreatureControler>();

            OnImpactObserver.AddUpdateMethod(TakeDamage);
            OnImpactObserver.AddUpdateMethod((int damage) => ActionHandler.AnimatorHandler.PlayTargetAnimation("Impact", 2));

            //Transforms
            TargetMark = creatureTargetMark;
        }

        private void Update() {

            ActionHandler.Update();
        }

        private void Death() {

            creatureControler.DisabledControler();
            IsAlive = false;
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
