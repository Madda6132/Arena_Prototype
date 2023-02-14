using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RPG.Actions;

namespace RPG.Creatures {
    public class AnimatorHandler {

        Animator animator;


        Action<int, string> OnAnimatorMessage;

        int horizontal;
        int vertical;


        public AnimatorHandler(Animator animator) {

            this.animator = animator;
            horizontal = Animator.StringToHash("Horizontal");
            vertical = Animator.StringToHash("Vertical");
            
            //Animations that triggers the method PerformTrigger
            //Animations that end Trigger a method here
        }

        public void Update() {
            
            //Uperbody layer
           
        }

        //Will crossfade not interrupt. Make a interrupter for Interrupt, impact and death
        public void StartAnimation(UtilityAnimations.AnimationInfo animationInfo) {
            
            //If animator is null or if animation name is empty, then exit
            if(!animator || String.IsNullOrEmpty(animationInfo.stateName)) return;

            #region SetTriggers
             
                for (int i = 0; i < animationInfo.triggers.Count; i++) {
                    animator.SetTrigger(animationInfo.triggers[i]);
                } 
            
            #endregion
            #region SetBools
            
                for (int i = 0; i < animationInfo.boolens.Count; i++) {
                    (string name, bool value) boolInfo = animationInfo.boolens[i];
                    animator.SetBool(boolInfo.name, boolInfo.value);
                }

            #endregion
            
            animator?.CrossFade(animationInfo.stateName, animationInfo.normalizedTransitionDuration, animationInfo.layer); 
        }

        //The message types are in AnimatorMessageType
        public void SubToAnimatorMessage(Action<int, string> listener) => OnAnimatorMessage += listener;
        public void UnsubToAnimatorMessage(Action<int, string> listener) => OnAnimatorMessage -= listener;

        /// <summary>
        /// Receive ActionActivate, ActionTrigger and ActionDeactivate for Action such as HitBox and perform ability
        /// </summary>
        public void AnimationMessageReciver(int layerIndex, string message) {

            OnAnimatorMessage?.Invoke(layerIndex, message);
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
        public bool GetAnimatorBool(string boolName) => animator.GetBool(boolName);

        //takes bool isInteraction and sets the value in the animator bool
        //Play animation
        public bool PlayTargetAnimation(string targetAnimation, int layer, float transitionDuration = 0.2f) {
            //animator.SetBool("IsInteracting", isInteracting);
            animator.CrossFade(targetAnimation, transitionDuration, layer);
            return true;
        }

        //Used to operate animation attack by referring to weapon and animation name ex "Swing"
        public void SetAnimatorTrigger(string triggerName) => animator.SetTrigger(triggerName);
        public void SetAnimatorBool(string boolName, bool state) => animator.SetBool(boolName, state);
        /*---Private---*/


    }

}
