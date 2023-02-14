using RPG.Creatures;
using System;

namespace RPG.Actions.Requirement {
    public class CreatureMovingRequirement : AbstractRequirement<bool> {

        protected override string ErrorMessage {
            get {

                return value == true ? _ErrorMessageTrue : _ErrorMessageFalse;
            }
        }
        /// <summary>
        /// Check if a creature is moving
        /// </summary>
        /// <param name="isMoving"> True = when Incapacitated, False = when not Incapacitated </param>
        /// <param name="action"> What action to call when listening call doesn't meet requirement </param>
        public CreatureMovingRequirement(bool isMoving, Action action) : base(isMoving, action) {

            _ErrorMessageFalse = "Can't do that while Moving";
            _ErrorMessageTrue = "Can't do that while not Moving";
        }

        public override void IgnoreRequirement(Creature creature) => RequirementMethods.IgnoreMoving(creature, Formula);
        public override void ListenToRequirement(Creature creature) => RequirementMethods.ListenToMoving(creature, Formula);

        /*---Protected---*/

        protected override void Formula(bool value) {

            if (this.value == value) action.Invoke();
        }

        protected override bool CheckIfAllowed(Creature creature) => RequirementMethods.CheckIfMoving(creature) == value;
    }
}