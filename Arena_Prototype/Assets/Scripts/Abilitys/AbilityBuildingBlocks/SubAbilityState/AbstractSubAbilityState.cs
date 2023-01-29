using UnityEngine; 

namespace RPG.Abilitys {
    /// <summary>
    /// The ability gets called when the ability state is made such as start/Tick/Target/End
    /// </summary>
    public class SubAbilityState {

        Ability ability;
        SubAbilityCall callState;
        Creatures.Creature user;
        int energy;

        public SubAbilityState(Creatures.Creature user, int energy, Ability ability, SubAbilityCall callState) {

            this.ability = ability;
            this.callState = callState;
            this.user = user;
            this.energy = energy;
        }

        public void Perform(AbstractFormBehavior formBehavior, Vector3 position) => 
            ability.PerformAbilityAtTargeting(new(ability, energy, user, position),
                position, formBehavior.transform.forward, formBehavior.transform.up);
        public void PerformTarget(AbstractFormBehavior formBehavior, GameObject[] targets) { 

            for (int i = 0; i < targets.Length; i++) {

                ability.PerformAbilityAtFormCreature(new(ability, energy, user, formBehavior.transform.position),
                formBehavior.transform.position, formBehavior.transform.forward, formBehavior.transform.up, targets[i]);
            } 
        }
        public SubAbilityCall GetCallState => callState;

        public enum SubAbilityCall {
            Start,
            Target,
            Tick,
            End
        }

    }

}
