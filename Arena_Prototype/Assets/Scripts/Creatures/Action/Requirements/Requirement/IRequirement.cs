using RPG.Creatures;

namespace RPG.Actions.Requirement {
    public interface IRequirement {
        /// <summary>
        /// If requirements are meet it return true
        /// </summary>
        public (bool isAllowed, string errorMessage) CheckRequirement(Creature creature);

        public void ListenToRequirement(Creature creature);

        public void IgnoreRequirement(Creature creature);

    }
}

