using RPG.Creatures;

namespace RPG.Actions.Requirement {
    /// <summary>
    /// Requirements preventing use
    /// </summary>
    public interface IRequirementCollection {

        /// <summary>
        /// If requirements are meet it return true
        /// </summary>
        public (bool isAllowed, string errorMessage) CheckRequirements(Creature creature);

        public void ListenToRequirements(Creature creature);

        public void IgnoreRequirements(Creature creature);

    }

}
