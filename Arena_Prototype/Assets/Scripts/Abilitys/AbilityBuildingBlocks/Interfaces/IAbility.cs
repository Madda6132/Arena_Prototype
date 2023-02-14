using UnityEngine;
using System.Collections.Generic;
using RPG.Abilitys.Form;

namespace RPG.Abilitys {
    public interface IAbility {

        public float GetRange(int energyCost);
        public int GetCost();
        /// <summary>
        /// Perform Ability from the start. Usually used during a attack trigger.
        /// </summary>
        /// <param name="abilityBaseInfo"> Information about startPoint, the user Creature, and energy </param>
        /// <param name="forwardDirection"> Usually, the direction the creature is facing. </param>
        /// <param name="upDirection"> The up direction for the ability.  </param>
        public AbstractFormBehavior[] PerformAbilityAtTargeting(Ability.AbilityBaseInfo abilityBaseInfo, Vector3 startPosition, Vector3 forwardDirection,
            Vector3 upDirection);
        public AbstractFormBehavior PerformAbilityAtFormCreature(Ability.AbilityBaseInfo abilityBaseInfo, Vector3 startPosition, Vector3 forwardDirection,
            Vector3 upDirection, GameObject creature);
        public AbstractFormBehavior PerformAbilityAtFormPosition(Ability.AbilityBaseInfo abilityBaseInfo, Vector3 startPosition, Vector3 forwardDirection,
            Vector3 upDirection, Vector3 position);
        public AbstractFormBehavior PerformAbilityAtFormDirection(Ability.AbilityBaseInfo abilityBaseInfo, Vector3 startPosition, Vector3 forwardDirection,
            Vector3 upDirection, Vector3 dir);
        public Effect.AbstractAbilityEffect[] GetEffects();

    }

}
