using RPG.Creatures;
using System;

namespace RPG.Actions.Requirement {
    public class CreatureIncapacitatedRequirement : AbstractRequirement<bool> {

        protected override string ErrorMessage {
            get {

                return value == true ? _ErrorMessageTrue : _ErrorMessageFalse;
            }
        }

        /// <summary>
        /// Check if a creature is incapacitated performing
        /// </summary>
        /// <param name="isIncapacitated"> True = when Incapacitated, False = when not Incapacitated </param>
        /// <param name="action"> What action to call when listening call doesn't meet requirement </param>
        public CreatureIncapacitatedRequirement(bool isIncapacitated, Action action) : base(isIncapacitated, action) {

            _ErrorMessageFalse = "Can't do that while Incapacitated";
            _ErrorMessageTrue = "Can't do that while not Incapacitated";
        }

        public override void IgnoreRequirement(Creature creature) => RequirementMethods.IgnoreIncapacitated(creature, Formula);
        public override void ListenToRequirement(Creature creature) => RequirementMethods.ListenToIncapacitated(creature, Formula);

        /*---Protected---*/

        protected override void Formula(bool value) {

            if (this.value == value) action.Invoke();
        }

        protected override bool CheckIfAllowed(Creature creature) => RequirementMethods.CheckIfIncapacitated(creature) == value;
    }
}