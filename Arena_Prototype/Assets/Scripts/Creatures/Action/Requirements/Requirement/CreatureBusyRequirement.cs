using RPG.Creatures;
using System;

namespace RPG.Actions.Requirement {
    public class CreatureBusyRequirement : AbstractRequirement<bool> {

        protected override string ErrorMessage {
            get {

                return value == true ? _ErrorMessageTrue : _ErrorMessageFalse;
            }
        }

        /// <summary>
        /// Check if a creatures performance handler is busy performing
        /// </summary>
        /// <param name="isBusy"> True = when busy, False = when not busy </param>
        /// <param name="action"> What action to call when listening call doesn't meet requirement </param>
        public CreatureBusyRequirement(bool isBusy, Action action) : base(isBusy, action) {

            _ErrorMessageFalse = "Can't do that while Busy";
            _ErrorMessageTrue = "Can't do that while not Busy";
        }
        public override void IgnoreRequirement(Creature creature) => RequirementMethods.IgnoreBusy(creature, Formula);
        public override void ListenToRequirement(Creature creature) => RequirementMethods.ListenToBusy(creature, Formula);

        /*---Protected---*/

        protected override void Formula(bool value) {

            if(this.value == value) action.Invoke();
        }

        protected override bool CheckIfAllowed(Creature creature) => RequirementMethods.CheckIfBusy(creature) == value;
    }
}
