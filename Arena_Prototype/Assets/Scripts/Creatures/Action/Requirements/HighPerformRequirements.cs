using RPG.Actions.Requirement;
using System;

namespace RPG.Actions {
    public class HighPerformRequirements : AbstractRequirementCollection {

        /// <summary>
        /// Requirements that keeps track of when creature isn't: busy, incapacitated, or moving
        /// </summary>
        /// <param name="action"> The action to call when requirements aren't meet </param>
        public HighPerformRequirements(Action action) {

            //Add Not Busy
            //Not Incapacitated
            //Don't Move
            this.requirements.Add(new CreatureBusyRequirement(false, action));
            this.requirements.Add(new CreatureIncapacitatedRequirement(false, action));
            this.requirements.Add(new CreatureMovingRequirement(false, action));
        }
    }

}
