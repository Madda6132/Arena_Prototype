using System;
using RPG.Actions.Requirement;

namespace RPG.Actions {
    public class MediumPerformRequirements : AbstractRequirementCollection {

        /// <summary>
        /// Requirements that keeps track of when creature isn't: busy or incapacitated
        /// </summary>
        /// <param name="action"> The action to call when requirements aren't meet </param>
        public MediumPerformRequirements(Action action) {

            //Add Not Busy
            //Not Incapacitated
            this.requirements.Add(new CreatureBusyRequirement(false, action));
            this.requirements.Add(new CreatureIncapacitatedRequirement(false, action));
        }
    }

}
