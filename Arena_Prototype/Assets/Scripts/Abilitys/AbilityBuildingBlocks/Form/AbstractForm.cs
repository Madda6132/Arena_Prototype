using UnityEngine;

namespace RPG.Abilitys.Form {
    /// <summary>
    /// A building block for Ability. Creates and implement FormBehavior for abilitys. As well as energy cost of Abilitys
    /// </summary>
    public abstract class AbstractForm : IEnergyCost {

        public float EnergyCost => procentAddedCost;
        protected float procentAddedCost = 0f;
        //FormObject projectile, Explosion, 
        protected TargetType targetingType;
        protected int energy;


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


        public TargetType GetTargetingType => targetingType;

        /*---Protected---*/

        /// <summary>
        /// Sets the targeting type and cost.
        /// The target type involve overlapping Gameobject, positions surrounding a form ex circle
        /// </summary>
        protected abstract void SetTargetTypeAndCost();

        protected Behavior CreateForm<Behavior>(Ability.AbilityBaseInfo abilityBaseInfo, Vector3 startPosition, 
            Vector3 forwardDirection) where Behavior : AbstractFormBehavior {

            GameObject gameObj = SOLibraryForms.GetForm<Behavior>(abilityBaseInfo.ability.GetElement).gameObject;
            Quaternion rotation = Quaternion.FromToRotation(gameObj.transform.forward, forwardDirection);
            return GameObject.Instantiate(gameObj, startPosition, rotation).GetComponent<Behavior>();
        }
    }
}

