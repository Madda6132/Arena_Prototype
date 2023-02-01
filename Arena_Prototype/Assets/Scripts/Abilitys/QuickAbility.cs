using RPG.Creatures;
using RPG.Combat;
using RPG.Actions;

namespace RPG.Abilitys {
    /// <summary>
    /// Fast Abilitys that wont require time to use
    /// </summary>
    public class QuickAbility  {

        Ability ability;

        public QuickAbility(AbilityElement abilityElement, int energy) {

            //Equipment sends element, energy
            Abilitys.Form.FormProjectile form = new(energy);
            ability = new(energy, abilityElement, TargetType.Direction, targeting: new Targeting.TargetingRaycast(energy), form: form);
        }

        public AbilityPerformAction GetAbilityAction(Creature creature, IAbilityTargetingObject targetingInfo) {

            return ability.GetAbilityAction(creature, targetingInfo);
             
        }
    }

}
