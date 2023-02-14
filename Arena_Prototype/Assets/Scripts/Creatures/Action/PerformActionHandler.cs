using UnityEngine;
using RPG.Creatures;
using System.Collections.Generic;

namespace RPG.Actions {
    public class PerformActionHandler {

        //User relevant
        Creature creature;
        public AnimatorHandler AnimatorHandler { get; }

        public List<IPerformAction> performanceActionList = new();
        IPerformAction currentPerformAction;

        //Is currently performing a action
        public ResultObserver<bool> IsBusyObserver = new();
        public bool IsBusy {

            get {
                return _IsBusy;
            }

            private set {
                _IsBusy = value;
                IsBusyObserver.OnResultUpdate(IsBusy);
            }
        }
        private bool _IsBusy = false;

        public PerformActionHandler(Creature creature) { 

            this.creature = creature;

            if(!creature.TryGetComponent(out Animator animator)) 
                animator = creature.GetComponentInChildren<Animator>();

            AnimatorHandler = new(animator);


        }

        public void Update() {

            AnimatorHandler.Update();
            performanceActionList.ForEach(x => x.Update());
        }

        /// <summary>
        /// Creature passes message -> Animator sends messages to this method
        /// </summary>
        public void AnimationMessageReciver(int layerIndex, string animationTrigger) => AnimatorHandler.AnimationMessageReciver(layerIndex, animationTrigger);

        public void StartAction(IPerformAction action) {

            
            (bool isAllowed, string errorMessage) feedback = action.CheckRequirements(creature);

            if (!feedback.isAllowed) {

                //Send failure message
                if (creature.TryGetComponent(out GameSystem.CreatureMessageDelegate messageDelegate))
                    messageDelegate.DelegateError(feedback.errorMessage);

                return;
            }

            if (action.CauseBusy) {

                IsBusy = action.CauseBusy;
                currentPerformAction?.Finish();
                currentPerformAction = action;
            }

            performanceActionList.Add(action);

            //returns name of animation 
            AnimatorHandler.StartAnimation(action.Perform(creature));
            
        }
        
        /// <summary>
        /// Cancel all active actions. (Usually called when incapacitated)
        /// </summary>
        public void Interupt() {
            
            List<IPerformAction> loopList = new(performanceActionList);

            loopList.ForEach(x => {
                x.Cancel();
                performanceActionList.Remove(x);
            });

            IsBusy = false;
        }

        public void ActionPerformFinished(IPerformAction performanceAction) {

            performanceAction?.Cancel();

            if(performanceActionList.Contains(performanceAction))
                performanceActionList.Remove(performanceAction);

            if(performanceAction.CauseBusy)
                IsBusy = false;
        }
    }

}
