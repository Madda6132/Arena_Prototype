using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilitys.Form {
    /// <summary>
    /// A building block for Ability. Creates and implement FormBehavior for abilitys. As well as energy cost of Abilitys
    /// </summary>
    public abstract class AbstractForm : IEnergyCost {

        public float EnergyCost => procentAddedCost;
        protected float procentAddedCost = 0f;
        protected int energy;

        AbstractFormBehavior _FormBehavior;
        Queue<AbstractFormBehavior> _FormBehaviorPool = new();

        public AbstractForm(int energy) {

            this.energy = energy;
           
            SetTargetTypeAndCost();
        }

        public abstract AbstractFormBehavior StartFormCreature(Ability.AbilityBaseInfo abilityBaseInfo,
            Vector3 startPosition, Vector3 forwardDirection, Vector3 upDirection, GameObject creature);
        public abstract AbstractFormBehavior StartFromPosition(Ability.AbilityBaseInfo abilityBaseInfo,
            Vector3 startPosition, Vector3 forwardDirection, Vector3 upDirection, Vector3 position);
        public abstract AbstractFormBehavior StartFromDirection(Ability.AbilityBaseInfo abilityBaseInfo,
            Vector3 startPosition, Vector3 forwardDirection, Vector3 upDirection, Vector3 direction);

        public void AddToPool(AbstractFormBehavior behavior) => _FormBehaviorPool.Enqueue(behavior);

        /*---Protected---*/

        /// <summary>
        /// Sets the targeting type and cost.
        /// The target type involve overlapping Gameobject, positions surrounding a form ex circle
        /// </summary>
        protected abstract void SetTargetTypeAndCost();

        protected Behavior GetObjectBehavior<Behavior>(Ability.AbilityBaseInfo abilityBaseInfo, Vector3 startPosition, 
            Vector3 forwardDirection) where Behavior : AbstractFormBehavior {

            if (_FormBehaviorPool.Count == 0) FillFormBehaviorPool<Behavior>(abilityBaseInfo.ability);

            GameObject gameObj = _FormBehaviorPool.Dequeue().gameObject;
            gameObj.transform.position = startPosition;
            gameObj.transform.rotation = Quaternion.LookRotation(forwardDirection);
            return gameObj.GetComponent<Behavior>();
        }

        /*---Private---*/

        private void FillFormBehaviorPool<Behavior>(Ability ability) where Behavior : AbstractFormBehavior {

            if (!_FormBehavior) {

                GameObject prefabGameObj = SOLibraryForms.GetForm<Behavior>(ability.GetElement).gameObject;
                prefabGameObj.SetActive(false);
                _FormBehavior = GameObject.Instantiate(prefabGameObj).GetComponent<Behavior>();
                Core.ContainerGameObject.AddToContainer<Behavior>(_FormBehavior.gameObject);
            }

            //Fill queue with 5 objects
            for (int i = 0; i < 5; i++) {

                AbstractFormBehavior formBehavior = GameObject.Instantiate(_FormBehavior).GetComponent<Behavior>();
                Core.ContainerGameObject.AddToContainer<Behavior>(formBehavior.gameObject);
                formBehavior.FillSettings(ability, this);
                _FormBehaviorPool.Enqueue(formBehavior);
            }
        }

    }
}

