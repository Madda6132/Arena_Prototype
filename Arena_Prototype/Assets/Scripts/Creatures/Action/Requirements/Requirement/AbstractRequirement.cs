using RPG.Creatures;
using System;

namespace RPG.Actions.Requirement {
    public abstract class AbstractRequirement<Value> : IRequirement {

        protected abstract string ErrorMessage { get; }
        protected Value value;
        protected Action action;

        protected string _ErrorMessageFalse = "Can't do that right now";
        protected string _ErrorMessageTrue = "Can't do that right now";


        public AbstractRequirement(Value value, Action action) {

            this.value = value;
            this.action = action;

        }

        public (bool isAllowed, string errorMessage) CheckRequirement(Creature creature) {

            bool isAllowed = CheckIfAllowed(creature);
            string message = isAllowed ? "" : ErrorMessage;

            return (isAllowed, message);
        }
        public abstract void IgnoreRequirement(Creature creature);
        public abstract void ListenToRequirement(Creature creature);

        /*---Protected---*/

        protected abstract bool CheckIfAllowed(Creature creature);
        protected abstract void Formula(Value value);
    }
}
