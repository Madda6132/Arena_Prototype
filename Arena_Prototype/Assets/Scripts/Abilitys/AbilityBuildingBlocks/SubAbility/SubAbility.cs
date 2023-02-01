using UnityEngine;
using System;

namespace RPG.Abilitys {
    /// <summary>
    /// The ability gets called when the ability state is made such as start/Tick/Target/End
    /// </summary>
    public class SubAbility {

        public enum SubAbilityCall {
            Random,
            Start,
            Target,
            Tick,
            End
        }

        Ability ability;
        SubAbilityCall callState;
        int energy;

        /// <summary>
        /// Create subAbilityState with set Ability
        /// </summary>
        /// <param name="energy"> The energy the Ability will have </param>
        /// <param name="ability"> The ability the SubAbility will call | Ignore to randomize </param>
        /// <param name="callState"> The call the SubAbility performs | Ignore to randomize </param>
        public SubAbility(int energy, Ability ability, SubAbilityCall callState = SubAbilityCall.Random) {

            this.ability = ability;
            this.callState = callState != SubAbilityCall.Random ? callState : SetRandomSubAbilityCall();
            this.energy = energy;
            
        }

        /// <summary>
        /// Create subAbilityState with random Ability
        /// </summary>
        /// <param name="energy"> The energy the Ability will have </param>
        /// <param name="element"> If ability is null then specify element for the randomized ability </param>
        /// <param name="callState"> The call the SubAbility performs | Ignore to randomize </param>
        public SubAbility(int energy, AbilityElement element, SubAbilityCall callState = SubAbilityCall.Random) {

            this.ability = new(energy, element);
            this.callState = callState != SubAbilityCall.Random ? callState : SetRandomSubAbilityCall();
            this.energy = energy;
        }


        public void PerformAtPosition(AbstractFormBehavior formBehavior, Vector3 position) => 
            ability.PerformAbilityAtTargeting(new(ability, energy, formBehavior.GetUser, position),
                position, formBehavior.transform.forward, formBehavior.transform.up);

        public void PerformAtTarget(AbstractFormBehavior formBehavior, GameObject[] targets) {

            Ability.AbilityBaseInfo abilityBaseInfo = new(ability, energy, formBehavior.GetUser, formBehavior.transform.position);
            Vector3 startPosition = formBehavior.transform.position;
            Vector3 forwardDirection = formBehavior.transform.forward;
            Vector3 upDirection = formBehavior.transform.up;

            for (int i = 0; i < targets.Length; i++) {

                ability.PerformAbilityAtFormCreature(abilityBaseInfo, startPosition, forwardDirection, upDirection, targets[i]);
            } 
        }

        public void SubToCall(AbstractFormBehavior formBehavior) {

            switch (callState) {
                case SubAbilityCall.Start:
                    formBehavior.SubToOnStartForm(PerformAtPosition);
                    break;
                case SubAbilityCall.Target:
                    formBehavior.SubToOnTriggeredForm(PerformAtTarget);
                    break;
                case SubAbilityCall.Tick:
                    formBehavior.SubToOnTickForm(PerformAtPosition);
                    break;
                default:
                case SubAbilityCall.End:
                    formBehavior.SubToOnEndForm(PerformAtPosition);
                    break;
            }
        }

        /*---Private---*/
        
        private SubAbilityCall SetRandomSubAbilityCall() {
            
            string[] callNames = Enum.GetNames(typeof(SubAbilityCall));
            int randomIndex = UnityEngine.Random.Range(1, callNames.Length);

            return (SubAbilityCall)Enum.Parse(typeof(SubAbilityCall), callNames[randomIndex]);
        }
    }

}
