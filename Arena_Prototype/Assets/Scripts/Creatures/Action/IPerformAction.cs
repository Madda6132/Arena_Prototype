using RPG.Creatures;

namespace RPG.Actions {
    public interface IPerformAction {

        public bool CauseBusy { get; }

        /// <summary>
        /// Start performing the Action
        /// </summary>
        /// <param name="creature"></param>
        /// <returns> Animation info </returns>
        public UtilityAnimations.AnimationInfo Perform(Creature creature);

        /// <summary>
        /// Finish the Action
        /// </summary>
        public void Finish();
        /// <summary>
        /// Check the requirements of the Action
        /// </summary>
        public (bool isAllowed, string errorMessage) CheckRequirements(Creature creature);

        /// <summary>
        /// Cancel the Action
        /// </summary>
        public void Cancel();

        public void Update();
    }

}
