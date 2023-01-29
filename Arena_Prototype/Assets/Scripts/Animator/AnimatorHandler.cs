using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RPG.Actions;

namespace RPG.Creatures {
    public class AnimatorHandler {

        Animator animator;

        //Trigger -> Usually set in the middle of an animation (Example want to cast a spell)
        Action performActionTrigger;
        //Activate -> Usually set at the start of an animation (Example activate weapon collider)
        Action performActionActivate;
        //Deactivate -> Usually set at the end of an animation (Example deactivate weapon collider)
        Action performActionDeactivate;


        int horizontal;
        int vertical;


        public AnimatorHandler(Animator animator) {

            this.animator = animator;
            horizontal = Animator.StringToHash("Horizontal");
            vertical = Animator.StringToHash("Vertical");
            //Animations that triggers the method PerformTrigger
            //Animations that end Trigger a method here
        }

        //Will crossfade not interrupt. Make a interrupter for Interrupt, impact and death
        public void StartAnimation(UtilityAnimations.AnimationInfo animationInfo) {

            //If animator is null exit
            if(!animator) return;

            #region SetTriggers
             
                for (int i = 0; i < animationInfo.triggers.Length; i++) {
                    animator.SetTrigger(animationInfo.triggers[i]);
                } 
            
            #endregion
            #region SetBools
            
                for (int i = 0; i < animationInfo.boolens.Length; i++) {
                    (string name, bool value) boolInfo = animationInfo.boolens[i];
                    animator.SetBool(boolInfo.name, boolInfo.value);
                }
            
            #endregion

            animator?.CrossFade(animationInfo.stateName, animationInfo.normalizedTransitionDuration, animationInfo.layer); 
        
        }

        //Trigger -> Usually set in the middle of an animation (Example want to cast a spell)
        #region Action Trigger
        /// <summary>
        /// Trigger -> Usually set in the middle of an animation (Example want to cast a spell)
        /// </summary>
        /// <param name="trigger">Action called when trigger is called </param>
        public void AddActionTriggerListener(Action trigger) => performActionTrigger += trigger;
        /// <summary>
        /// Trigger -> Usually set in the middle of an animation (Example want to cast a spell)
        /// </summary>
        /// <param name="trigger"> Action called when trigger is called </param>
        public void RemoveActionTriggerListener(Action trigger) => performActionTrigger -= trigger;

        #endregion

        //Activate -> Usually set at the start of an animation (Example activate weapon collider)
        #region Action Activate
        /// <summary>
        /// Activate -> Usually set at the start of an animation (Example activate weapon collider)
        /// </summary>
        /// <param name="trigger"> Action called when trigger is called </param>
        public void AddActionActivateListener(Action trigger) => performActionActivate += trigger;
        /// <summary>
        /// Activate -> Usually set at the start of an animation (Example activate weapon collider)
        /// </summary>
        /// <param name="trigger"> Action called when trigger is called </param>
        public void RemoveActionActivateListener(Action trigger) => performActionActivate -= trigger;

        #endregion

        //Deactivate -> Usually set at the end of an animation (Example deactivate weapon collider)
        #region Action Deactivate
        /// <summary>
        /// Deactivate -> Usually set at the end of an animation (Example deactivate weapon collider)
        /// </summary>
        /// <param name="trigger"> Action called when trigger is called </param>
        public void AddActionDeactivateListener(Action trigger) => performActionDeactivate += trigger;
        /// <summary>
        /// Deactivate -> Usually set at the end of an animation (Example deactivate weapon collider)
        /// </summary>
        /// <param name="trigger"> Action called when trigger is called </param>
        public void RemoveActionDeactivateListener(Action trigger) => performActionDeactivate -= trigger;

        #endregion

        /// <summary>
        /// Receive ActionActivate, ActionTrigger and ActionDeactivate for Action such as HitBox and perform ability
        /// </summary>
        public void PerformTrigger(string animationTrigger) {
            Debug.Log("Perform Trigger name " + animationTrigger);

            switch (animationTrigger) {
                case "ActionActivate":
                    Debug.Log("Perform Trigger ActionActivate");
                    performActionActivate?.Invoke(); 
                    break;
                case "ActionTrigger":
                    Debug.Log("Perform Trigger ActionTrigger");
                    performActionTrigger?.Invoke(); 
                    break;
                case "ActionDeactivate":
                    Debug.Log("Perform Trigger ActionDeactivate");
                    performActionDeactivate?.Invoke();
                    break;
                default:
                    break;
            }

        }

        public void UpdateAnimatorMovementValues(float horizontalMovement, float verticalMoment, bool isSprinting) {
            //Movement Snapping
            float snappedHorizontal;
            float snappedVertical;

            #region SnappedHorizontalMovement
            if (horizontalMovement > 0 && horizontalMovement < 0.55f) {
                snappedHorizontal = 0.5f;
            } else if (horizontalMovement > 0.55f) {
                snappedHorizontal = 1;
            } else if (horizontalMovement < 0 && horizontalMovement > -0.55f) {
                snappedHorizontal = -0.5f;
            } else if (horizontalMovement < -0.55f) {
                snappedHorizontal = -1;
            } else {
                snappedHorizontal = 0;
            }
            #endregion

            #region SnappedVerticalMoment
            if (verticalMoment > 0 && verticalMoment < 0.55f) {
                snappedVertical = 0.5f;
            } else if (verticalMoment > 0.55f) {
                snappedVertical = 1;
            } else if (verticalMoment < 0 && verticalMoment > -0.55f) {
                snappedVertical = -0.5f;
            } else if (verticalMoment < -0.55f) {
                snappedVertical = -1;
            } else {
                snappedVertical = 0;
            }

            #endregion

            if (isSprinting) {
                snappedHorizontal = horizontalMovement;
                snappedVertical *= 2;
            }
            animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
            animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
        }


        //Checks if a animation is active usually to interrupt new animations
        public bool GetAnimatorBool(string boolName) {
            return animator.GetBool(boolName);
        }

        //takes bool isInteraction and sets the value in the animator bool
        //Play animation
        public bool PlayTargetAnimation(string targetAnimation, int layer, float transitionDuration = 0.2f) {
            //animator.SetBool("IsInteracting", isInteracting);
            animator.CrossFade(targetAnimation, transitionDuration, layer);
            return true;
        }

        //Used to operate animation attack by referring to weapon and animation name ex "Swing"
        public void SetAnimatorTrigger(string triggerName) {
            animator.SetTrigger(triggerName);
        }
    }

}
