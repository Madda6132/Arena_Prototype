using UnityEngine;
using RPG.Creatures;

namespace RPG.Actions {
    public interface IPerformAction {

        /// <summary>
        /// Start performing the Action.
        /// Returns the animation name.
        /// </summary> 
        public UtilityAnimations.AnimationInfo Perform(Creature creature);

        /// <summary>
        /// Finish the Action
        /// </summary>
        public void Finish();
        /// <summary>
        /// Check the requirements of the Action
        /// </summary>
        public bool Requirements();

        /// <summary>
        /// Cancel the Action
        /// </summary>
        public void Cancel();
    }

}
