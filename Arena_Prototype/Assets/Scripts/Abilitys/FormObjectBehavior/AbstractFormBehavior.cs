using System.Collections.Generic;
using UnityEngine;
using RPG.Abilitys.Form;
using RPG.Abilitys;
using System;
using RPG.Creatures;
using RPG.Abilitys.Effect;
using System.Linq;


/// <summary>
/// The base class of all FormBehaviors
/// </summary>
namespace RPG.Abilitys.Form {
    public abstract class AbstractFormBehavior : MonoBehaviour, IFormBehavior {
        public enum OnCall {
            OnStart,
            OnTrigger,
            OnTick,
            OnEnd
        }

        [Tooltip("During the lifetime of the behavior, call OnTickForm every tickTimer in seconds")]
        [Range(0.05f, 120f)]
        [SerializeField] float tickTimer = 1f;

        [Tooltip("OnStart and OnEnd plays particals. \n" + "While OnTrigger and OnTick creates and deletes effects")]
        [SerializeField] List<ParticleEffectTask> particleEffectTasks = new List<ParticleEffectTask>();

        //Contains filters, perks and message listeners (Counter, and such)
        Dictionary<Type, List<object>> ActionDictinary = new();

        //FormBehavior targeting effectLists (Such as Projectile hitting a target)
        Action<AbstractFormBehavior, GameObject[]> gameObjectListeners;
        Action<AbstractFormBehavior, Vector3[]> positionListeners;
        Action<AbstractFormBehavior, Vector3[]> directionListeners;

        //Action<... , Vector3> => Position of start, tick and end
        protected Action<AbstractFormBehavior, Vector3> OnStartForm;
        protected Action<AbstractFormBehavior, GameObject[]> OnTriggeredForm;
        protected Action<AbstractFormBehavior, Vector3> OnTickForm;
        protected Action<AbstractFormBehavior, Vector3> OnEndForm;

        protected AbstractForm form;
        protected Ability.AbilityBaseInfo abilityBaseInfo;

        protected bool isInteruptingAction = false;

        protected bool _IsLifeIsEnding = false;

        protected float _LifeTime = 6f;
        float _TimePast = 0;
        float _TickTracker = 0f;

        //Once the startup is finished call, start form
        public virtual void StartForm(Ability.AbilityBaseInfo abilityBaseInfo) {

            this.abilityBaseInfo = abilityBaseInfo;

            //Clear Lists
            foreach (var item in ActionDictinary) {
                item.Value.Clear();
            }

            //Fill lists
            foreach (var perk in abilityBaseInfo.ability.GetPerks) {
                object perkStorage = perk.GetPerkStorage(this);
                switch (perkStorage) {
                    case IFilter:
                        AddToDictonary(typeof(IFilter), perkStorage);
                        break;
                    case IActionMessageListener:
                        AddToDictonary(typeof(IActionMessageListener), perkStorage);
                        break;
                    default:
                        break;
                }
            }

            _IsLifeIsEnding = false;

            _TimePast = 0;
            _TickTracker = tickTimer;

            gameObject.SetActive(true);

            OnStartForm.Invoke(this, GetStartPosition());

        }

        public void FillSettings(Ability ability, AbstractForm form) {

            this.form = form;

            //Ability get any and all perks and sub abilitys
            foreach (var subAbility in ability.GetSubAbilitys) {

                subAbility.SubToCall(this);
            }

            ActionDictinary[typeof(IFilter)] = new();
            ActionDictinary[typeof(IActionMessageListener)] = new();

            //Effects happens when the form sends info
            foreach (var effect in ability.GetEffects()) {

                switch (effect) {
                    case IGameObjectEffect:
                        gameObjectListeners += ((IGameObjectEffect)effect).PerformEffectOnObjects;
                        break;
                    case IPositionEffect:
                        positionListeners += ((IPositionEffect)effect).PerformEffectOnPosition;
                        break;
                    case IDirectionEffect:
                        directionListeners += ((IDirectionEffect)effect).PerformEffectOnDirection;
                        break;
                    default:
                        break;
                }
            }

        }

        public Creature GetUser => abilityBaseInfo.user;
        public int GetEnergy => abilityBaseInfo.energy;
        public void InteruptAction() => isInteruptingAction = true;

        //Override to change alter the direction of the Form
        public virtual void Redirect(Vector3 forwardDirection, Vector3 upDirection) { }
        //Repeat effect
        public virtual AbstractFormBehavior[] Repeat() =>
            abilityBaseInfo.ability.PerformAbilityAtTargeting(abilityBaseInfo, transform.position, transform.forward, transform.up).ToArray();

        /// <summary>
        /// Get random positions from the Form Behavior
        /// </summary>
        public abstract Vector3[] GetTargetPositions();
        /// <summary>
        /// Get random directions from the Form Behavior
        /// </summary>
        public virtual Vector3[] GetTargetDirections() {

            Vector3[] positions = GetTargetPositions();
            Vector3[] directions = new Vector3[positions.Length];

            for (int i = 0; i < positions.Length; i++) {
                directions[i] = positions[i] - transform.position;
            }
            return directions;
        }

        /// <summary>
        /// Get GameObjects from the Form Behavior
        /// </summary>
        public abstract GameObject[] GetTargetObjects();

        //Replace a storage. (Repeat uses this to remember the nummber of times to repeat)
        public void ReplaceStorage<StorageType>(StorageType storage) where StorageType : class {

            if (storage == null) return;


            Action<List<object>> Replace = storageList => {

                foreach (var item in storageList) {

                    if (item.GetType() == typeof(StorageType)) {
                        storageList.Remove(item);
                        break;
                    }
                }

                storageList.Add(storage);
            };

            switch (storage) {
                case IFilter:
                    if (ActionDictinary.ContainsKey(typeof(IFilter)))
                        Replace(ActionDictinary[typeof(IFilter)]);
                    break;

                case IActionMessageListener:
                    if (ActionDictinary.ContainsKey(typeof(IActionMessageListener)))
                        Replace(ActionDictinary[typeof(IActionMessageListener)]);
                    break;
                default:
                    break;

            }
        }

        #region Subscriptions

        //Subscribe to events
        public void SubToOnStartForm(Action<AbstractFormBehavior, Vector3> listener) =>
            OnStartForm += listener;
        public void SubToOnTriggeredForm(Action<AbstractFormBehavior, GameObject[]> listener) =>
            OnTriggeredForm += listener;
        public void SubToOnTickForm(Action<AbstractFormBehavior, Vector3> listener) =>
            OnTickForm += listener;
        public void SubToOnEndForm(Action<AbstractFormBehavior, Vector3> listener) =>
            OnEndForm += listener;
        //Unsubscripted to events
        public void UnsubToOnStartForm(Action<AbstractFormBehavior, Vector3> listener) =>
            OnStartForm -= listener;
        public void UnsubToOnTriggeredForm(Action<AbstractFormBehavior, GameObject[]> listener) =>
            OnTriggeredForm -= listener;
        public void UnsubToOnTickForm(Action<AbstractFormBehavior, Vector3> listener) =>
            OnTickForm -= listener;
        public void UnsubToOnEndForm(Action<AbstractFormBehavior, Vector3> listener) =>
            OnEndForm -= listener;

        #endregion

        /*---Protected---*/

        protected abstract void ExtraUpdate();
        protected abstract Vector3 GetStartPosition();
        protected abstract Vector3 GetEndPosition();
        protected abstract Vector3 GetTickPosition();

        //Starting the process of ending the behavior and gameObject
        protected virtual void StartingEndingFormBehavior() {


            //Send the message to listeners to see if they interrupt the process
            isInteruptingAction = false;

            foreach (var listener in ActionDictinary[typeof(IActionMessageListener)]) {

                ((IActionMessageListener)listener).Perform(this, FormUtilityMessages.START_DESTROY);
            }

            if (!isInteruptingAction) EndFormBehavior();

            isInteruptingAction = false;

        }

        //Is now ending the FormBehavior
        protected virtual void EndFormBehavior() {

            if (_IsLifeIsEnding) return;

            _IsLifeIsEnding = true;

            foreach (var listener in ActionDictinary[typeof(IActionMessageListener)]) {

                ((IActionMessageListener)listener).Perform(this, FormUtilityMessages.END);
                ((IActionMessageListener)listener).Perform(this, FormUtilityMessages.DESTROY);
            }

            OnEndForm?.Invoke(this, GetEndPosition());
            form.AddToPool(this);
            gameObject.SetActive(false);
        }

        // Senders send information to triggered listeners
        #region Senders

        protected void SendGameObject(params GameObject[] targets) {

            foreach (IFilter filter in ActionDictinary[typeof(IFilter)]) {

                targets = filter.Filter(targets);
            }

            if (targets.Length == 0) return;

            //Position
            Vector3[] positions = ConvertGameObjectToPosition(targets);

            //Direction
            Vector3[] directions = ConvertPositionToDirection(positions);

            SendTrigger(targets, positions, directions);
        }
        /// <summary>
        /// Senders send information to triggered listeners
        /// </summary>
        protected void SendPosition(params Vector3[] positions) {

            //GameObject
            GameObject[] targets = ConvertPositionToGameObject(positions);

            //Direction
            Vector3[] directions = ConvertPositionToDirection(positions);

            SendTrigger(targets, positions, directions);
        }
        /// <summary>
        /// Senders send information to triggered listeners
        /// </summary>
        protected void SendDirection(params Vector3[] directions) {

            //Position
            Vector3[] positions = ConvertDirectionToPosition(directions);

            //GameObject
            GameObject[] targets = ConvertPositionToGameObject(positions);


            SendTrigger(targets, positions, directions);
        }

        //---Private---

        /// <summary>
        /// Performs repetitive actions from senders 
        /// </summary>
        private void SendTrigger(GameObject[] targets, Vector3[] positions, Vector3[] directions) {

            foreach (IFilter filter in ActionDictinary[typeof(IFilter)]) {

                targets = filter.Filter(targets);
            }

            foreach (var listener in ActionDictinary[typeof(IActionMessageListener)]) {

                ((IActionMessageListener)listener).Perform(this, FormUtilityMessages.TRIGGER);
            }

            positionListeners?.Invoke(this, positions);
            gameObjectListeners?.Invoke(this, targets);
            directionListeners?.Invoke(this, directions);


            OnTriggeredForm?.Invoke(this, targets);
        }
        #endregion

        /*---Private---*/


        private void OnDrawGizmosSelected() {
            Debug.DrawRay(transform.position, transform.forward, Color.red);
        }

        private void Awake() {
            //ParticleEffects a behavior will perform during it's lifetime
            particleEffectTasks.ForEach(x => x.SubToCall(this));

        }

        private void Update() {

            _TimePast += Time.deltaTime;

            if (_LifeTime < _TimePast) EndFormBehavior();

            if (_TickTracker < _TimePast) {

                _TickTracker += tickTimer;

                foreach (var listener in ActionDictinary[typeof(IActionMessageListener)]) {

                    ((IActionMessageListener)listener).Perform(this, FormUtilityMessages.TICK);
                }

                OnTickForm?.Invoke(this, GetTickPosition());
            }

            ExtraUpdate();
        }


        /// <summary>
        /// Add memory, filters, and message listeners (Counter, and such)
        /// </summary>
        /// <param name="key">Class type to store. Can only have one type at the time </param>
        /// <param name="storage">The storage for memory and what action to perform</param>
        private void AddToDictonary(Type key, object storage) {

            if (ActionDictinary.ContainsKey(key)) {

                ActionDictinary[key].Add(storage);
            } else {

                ActionDictinary.Add(key, new());
                ActionDictinary[key].Add(storage);
            }
        }

        #region Converters

        //Convert
        /// <summary>
        /// Return the closests gameobject
        /// </summary>
        /// <param name="position"> Check GameObjects with the overlap sphere</param>
        /// <param name="SphereSize"> The size of the overlap sphere</param>
        /// <returns></returns>
        private GameObject[] ConvertPositionToGameObject(Vector3[] position, float SphereSize = 0.1f) {

            //Avoid adding the same gameobject
            List<GameObject> _GameObjectsFound = new();

            for (int i = 0; i < position.Length; i++) {
                Collider[] colliders = Physics.OverlapSphere(position[i], SphereSize);
                colliders = colliders.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).ToArray();

                GameObject[] _Targets = colliders.Select(x => x.gameObject).ToArray();
                for (int y = 0; y < _Targets.Length; y++) {

                    if (!_GameObjectsFound.Contains(_Targets[y]))
                        _GameObjectsFound.Add(_Targets[y]);

                }
            }

            return _GameObjectsFound.ToArray();
        }

        private Vector3[] ConvertGameObjectToPosition(GameObject[] targets) {

            List<Vector3> positions = new();
            for (int i = 0; i < targets.Length; i++) {

                if (targets[i].TryGetComponent(out Creature creature)) {

                    positions.Add(creature.TargetMark.position);
                } else {

                    positions.Add(targets[i].transform.position);
                }
            }

            return positions.ToArray();
        }
        private Vector3[] ConvertPositionToDirection(Vector3[] position) {

            Vector3[] directions = new Vector3[position.Length];
            for (int i = 0; i < position.Length; i++) {

                directions[i] = (position[i] - transform.position).normalized;
            }
            return directions;
        }
        private Vector3[] ConvertDirectionToPosition(Vector3[] direction) {

            Vector3[] Position = new Vector3[direction.Length];
            for (int i = 0; i < direction.Length; i++) {

                Position[i] = direction[i] + transform.position;
            }

            return Position;
        }

        #endregion

        [System.Serializable]
        public class ParticleEffectTask {

            [SerializeField] ParticleSystem particle;
            [SerializeField] OnCall call;


            public void SubToCall(AbstractFormBehavior behavior) {

                switch (call) {
                    case OnCall.OnStart:
                        behavior.SubToOnStartForm((formBehavior, position) => {
                            particle.transform.position = position;
                            particle.transform.localScale = formBehavior.transform.localScale;
                            particle.Play();
                        });
                        break;
                    case OnCall.OnTrigger:
                        behavior.SubToOnTriggeredForm((formBehavior, targets) => {
                            foreach (var target in targets) {

                                CreateTemporarilyEffect(formBehavior, target.transform.position, behavior.transform);
                            }
                        });

                        break;
                    case OnCall.OnTick:
                        behavior.SubToOnTickForm((formBehavior, position) =>
                            CreateTemporarilyEffect(formBehavior, position, behavior.transform));
                        break;
                    case OnCall.OnEnd:
                        behavior.SubToOnEndForm((formBehavior, position) => {
                            particle.transform.position = position;
                            particle.transform.localScale = formBehavior.transform.localScale;
                            particle.Play();
                        });
                        break;
                    default:
                        break;
                }
            }

            //Create a temporary particle system that destroys itself after it finishes
            public void CreateTemporarilyEffect(AbstractFormBehavior behavior, Vector3 position = default, Transform parentTransfrom = null) {
                ParticleSystem newEffect;

                if (parentTransfrom == null) {

                    newEffect = Instantiate(particle.gameObject).GetComponent<ParticleSystem>();
                } else {

                    newEffect = Instantiate(particle.gameObject, behavior.transform).GetComponent<ParticleSystem>();
                }

                newEffect.transform.position = position == default ? behavior.transform.position : position;
                newEffect.transform.localScale = behavior.transform.localScale;
                newEffect.Play();
                Destroy(newEffect.gameObject, newEffect.main.duration);
            }

        }
    }
}