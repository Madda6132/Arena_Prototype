using UnityEngine;
using RPG.Creatures;

namespace RPG.Actions {
    public class PerformActionHandler {

        //User relevant
        Creature creature;
        public AnimatorHandler AnimatorHandler { get; }

        //Is currently performing a action
        public bool isBusy { get; private set; } = false;

        public IPerformAction performanceAction { get; private set; }

        
        public PerformActionHandler(Creature creature) { 

            this.creature = creature;

            if(!creature.TryGetComponent(out Animator animator)) 
                animator = creature.GetComponentInChildren<Animator>();

            AnimatorHandler = new(animator);

        }


        /// <summary>
        /// Creature passes message -> Animator sends messages to this method
        /// </summary>
        public void AnimationReciver(string animationTrigger) => AnimatorHandler.PerformTrigger(animationTrigger);

        public void StartAction(IPerformAction action) {

            //Send failure message
            if (isBusy || creature.incapacitated || performanceAction != null ||!action.Requirements()) return;


            //creatureAction?.Finish(); Might be used for Channel Ability or rapid fire
            performanceAction = action;

            //returns name of animation 
            AnimatorHandler.StartAnimation(action.Perform(creature));
            isBusy = true;


        }
        
        public void Interupt() {

            performanceAction?.Cancel();
            performanceAction = null;
            isBusy = false;
        }

        public void ActionAnimationFinished() {

            performanceAction?.Cancel();
            performanceAction = null;
            isBusy = false;
        }
    }

}
