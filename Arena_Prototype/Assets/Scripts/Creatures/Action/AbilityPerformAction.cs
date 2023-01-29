using RPG.Creatures;
using UnityEngine;
using System;
using RPG.Combat;

namespace RPG.Actions {
    /// <summary>
    /// A storage of information for a perform action
    /// </summary>
    public class AbilityPerformAction : IPerformAction {

        Creature user;
        int energy;
        public IAbilityTargetingObject targetingInfo { get; private set; }
        public Abilitys.Ability Ability { get; private set; }
        Func<Creature, Transform, (Vector3 forwardDirection, Vector3 upDirection)> getDirection;
        
        //Trigger the ability to perform when called
        Action performAbility;
        //Trigger the ability to perform with a target when called
        Action<GameObject> triggerAbility;

        //Handles Subscriptions for when the action is
        //subAction -> Starting the performance
        //unsubAction -> Ending the performance
        Action<Creature, AbilityPerformAction, Action> subAction;
        Action<Creature, AbilityPerformAction, Action> unsubAction;

        //Animation information
        UtilityAnimations.AnimationInfo animationInfo;


        public AbilityPerformAction(Abilitys.Ability ability, Creature user, IAbilityTargetingObject equipment) {
            this.user = user;
            this.targetingInfo = equipment;
            this.Ability = ability;

            Ability.GetTargeting.FillPerformanceTargetingInfo(this);
        }

        /// <summary>
        /// Set the animation and the animator information
        /// </summary>
        public void SetAnimationInfo(UtilityAnimations.AnimationInfo animationInfo) => 
            this.animationInfo = animationInfo;

        public void SetPerformAbility(Action<Abilitys.Ability.AbilityBaseInfo,
            Vector3, Vector3, Vector3> performAbility) {

            this.performAbility = () => {
                Transform startPosition = targetingInfo.TargetingTransform;
                (Vector3 forwardDirection, Vector3 upDirection) directionInfo = getDirection(user, startPosition);
                performAbility.Invoke(GetAbilityBaseInfo, startPosition.position, directionInfo.forwardDirection, directionInfo.upDirection);
                
            };
        }

        public void SetTriggerAbility(Action<Abilitys.Ability.AbilityBaseInfo, Vector3, 
            Vector3, Vector3, GameObject> triggerAbility) {
            
            this.triggerAbility = (target) => {
                Transform startPoint = targetingInfo.TargetingTransform;
                (Vector3 forwardDirection, Vector3 upDirection) directionInfo = getDirection(user, startPoint);
                triggerAbility.Invoke(GetAbilityBaseInfo, startPoint.position, directionInfo.forwardDirection, directionInfo.upDirection, target);
                
            };
        }
        
        /// <summary>
        /// Energy that modify the ability
        /// </summary>
        public void SetEnergy(int energy) => this.energy = energy;

        /// <summary>
        /// Set the function of the actions direction. (Example character looking direction)
        /// </summary>
        /// <param name="getDirection"> Function of the direction </param>
        public void SetDirectionFunction(Func<Creature, Transform, (Vector3 forwardDirection, Vector3 upDirection)> getDirection) => this.getDirection = getDirection;
        /// <summary>
        /// When Action starts performing it calls the action SubAction
        /// </summary>
        /// <param name="subAction"> The subscriptions to add </param>
        public void SetSubAction(Action<Creature, AbilityPerformAction,  Action> subAction) => this.subAction = subAction;
        /// <summary>
        /// When Action ends or canceled it calls the action unsubAction
        /// </summary>
        /// <param name="unsubAction"> The subscriptions to remove </param>
        public void SetUnsubAction(Action<Creature, AbilityPerformAction, Action> unsubAction) => this.unsubAction = unsubAction;
        
        public void Cancel() {

            StartUnsubAction();
        }

        public void Finish() {
            performAbility();
            Cancel();
        }

        /// <summary>
        /// Used for TargetingHitBox to receive targets being hit
        /// </summary>
        public void PerformTriggerAbility(GameObject target) => triggerAbility?.Invoke(target);

        //Once Perform Action handler allows this action it starts this process 
        public UtilityAnimations.AnimationInfo Perform(Creature creature) {

            StartSubAction();
            return animationInfo;
        }

        //Checks requirements for the ability
        //Yet to be implemented
        public bool Requirements() {

            Debug.Log("Check requirements");
            return true;
        }

        //Animation Triggers
        //Lister to Animation ActionActivate
        //This occurs ex when Weapon hitbox should activate or when to fire of an ability

        /// <summary>
        /// Activate -> Usually set at the start of an animation (Example activate weapon collider)
        /// </summary>
        /// <param name="trigger"> Action called when trigger is called </param>
        public void SubToActionActivate(Action Lister) => AnimationHandler.AddActionActivateListener(Lister);
        /// <summary>
        /// Activate -> Usually set at the start of an animation (Example activate weapon collider)
        /// </summary>
        /// <param name="trigger"> Action called when trigger is called </param>
        public void UnsubToActionActivate(Action Lister) => AnimationHandler.RemoveActionActivateListener(Lister);
        /// <summary>
        /// Deactivate -> Usually set at the end of an animation (Example deactivate weapon collider)
        /// </summary>
        /// <param name="trigger"> Action called when trigger is called </param>
        public void SubToActionDeactivate(Action Lister) => AnimationHandler.AddActionDeactivateListener(Lister);
        /// <summary>
        /// Deactivate -> Usually set at the end of an animation (Example deactivate weapon collider)
        /// </summary>
        /// <param name="trigger"> Action called when trigger is called </param>
        public void UnsubToActionDeactivate(Action Lister) => AnimationHandler.RemoveActionDeactivateListener(Lister);
        /// <summary>
        /// Trigger -> Usually set in the middle of an animation (Example want to cast a spell)
        /// </summary>
        /// <param name="trigger">Action called when trigger is called </param>
        public void SubToAnimationActionTrigger(Action Lister) => AnimationHandler.AddActionTriggerListener(Lister);
        /// <summary>
        /// Trigger -> Usually set in the middle of an animation (Example want to cast a spell)
        /// </summary>
        /// <param name="trigger"> Action called when trigger is called </param>
        public void UnsubToAnimationActionTrigger(Action Lister) => AnimationHandler.RemoveActionTriggerListener(Lister);

        //Equipment Triggers
        /// <summary>
        /// Add to listen to a collider when it is triggered
        /// </summary>
        /// <param name="Lister"> The action that will revive the triggered target </param>
        public void SubToEquipmentColliderTrigger(Action<GameObject> Lister) => targetingInfo.SubToColliderTrigger(Lister);
        /// <summary>
        /// Remove from listen to a collider when it is triggered
        /// </summary>
        /// <param name="Lister"> The action that will revive the triggered target </param>
        public void UnsubToEquipmentColliderTrigger(Action<GameObject> Lister) => targetingInfo.UnsubToColliderTrigger(Lister);

        //---Private---

        //When Action is performing subscribe to the required events
        private void StartSubAction() => subAction?.Invoke(user, this, performAbility);

        //When Action is canceled/Finished unsubscripted to the required events
        private void StartUnsubAction() => unsubAction?.Invoke(user, this, performAbility);

        private Abilitys.Ability.AbilityBaseInfo GetAbilityBaseInfo => 
            new(Ability, energy, user, targetingInfo.TargetingTransform.position);
        
        private AnimatorHandler AnimationHandler => user.ActionHandler.AnimatorHandler;
        
        
    }

}
