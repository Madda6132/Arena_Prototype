using RPG.Creatures;
using System.Linq;
using System.Collections.Generic;

namespace RPG.Actions.Requirement {
    public abstract class AbstractRequirementCollection : IRequirementCollection {

        protected List<IRequirement> requirements = new();


        public (bool isAllowed, string errorMessage) CheckRequirements(Creature creature) {

            //Check every requirement
            foreach (var requirement in requirements) {

                (bool isAllowed, string errorMessage) feedBack = requirement.CheckRequirement(creature);

                //If requirement return false then send back error message
                if (!feedBack.isAllowed) return feedBack;
            }

            //Passed and no message
            return (true, "");
        }

        public void IgnoreRequirements(Creature creature) => requirements.ForEach(requirement => 
            requirement.IgnoreRequirement(creature));

        public void ListenToRequirements(Creature creature) => requirements.ForEach(requirement =>
            requirement.ListenToRequirement(creature));
    }
}

