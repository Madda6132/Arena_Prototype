using UnityEngine;
using RPG.Creatures;
using System;
using RPG.Actions;

namespace RPG.Abilitys.Targeting {
    /// <summary>
    /// A building block for Ability. Handles Ability targeting and how to perform the ability
    /// </summary>
    public abstract class AbstractTargeting : IEnergyCost {

        public float EnergyCost => procentAddedCost;
        protected float procentAddedCost = 0f;

        //Swing for now, can randomize for other?
        protected UtilityAnimations.AnimationInfo animationInfo = UtilityAnimations.AttackSwing;
        public AbstractTargeting(int energy) {

        }
        
        public virtual void FillPerformanceTargetingInfo(AbilityPerformAction performanceAction) {

            Func<Creature, Transform, (Vector3 forwardDirection, Vector3 upDirection)> getDirection = (user, equipment) => {

                
                (Vector3 upDirection, Ray ray, RaycastHit hit) rayInformation = user.GetRayLook();

                Vector3 hitPosition = rayInformation.hit.collider != null ?
                    rayInformation.hit.point : rayInformation.ray.GetPoint(30f);
                 
                Vector3 aimDirection = hitPosition - equipment.position;
                return (aimDirection.normalized, rayInformation.upDirection);
            };

            performanceAction.SetDirectionFunction(getDirection);

            //Set the Subscriptions on the performanceAction if the action is executed
            performanceAction.SetSubAction(SubToPerformAction);
            //Set the unsubscriptions on the performanceAction if the action finishes or canceled
            performanceAction.SetUnsubAction(UnsubToPerformAction);
            performanceAction.SetAnimationInfo(animationInfo);

            //New Action to circumvent return value
            Action<Ability.AbilityBaseInfo, Vector3, Vector3, Vector3> perAction = (abilityBaseInfo, startPos, forwardDir, upDir) => {
                Ability ability = performanceAction.Ability;
                ability.PerformAbilityAtTargeting(abilityBaseInfo, startPos, forwardDir, upDir);
            };

            //Set the action to take when the attack animation triggers
            performanceAction.SetPerformAbility(perAction);
        }

        
        //In the future optimize it to take a monobehavior to target
        //Each Class that inheret this class needs to get information on these three values
        public abstract GameObject[] TargetObject(Ability.AbilityBaseInfo abilityBaseInfo, Vector3 forwardDirection,
            Vector3 upDirection);
        public abstract Vector3[] TargetPosition(Ability.AbilityBaseInfo abilityBaseInfo, Vector3 forwardDirection,
            Vector3 upDirection);
        public abstract Vector3[] TargetDirection(Ability.AbilityBaseInfo abilityBaseInfo, Vector3 forwardDirection,
            Vector3 upDirection);

        /*---Protected---*/

        /// <summary>
        /// Change sub to activate collider
        /// </summary>
        protected virtual void SubToPerformAction(Creature user, AbilityPerformAction performanceAction,
            Action performAbility) {
            //Add listener on animator trigger
            AnimatorHandler animatiorHandler = user.ActionHandler.AnimatorHandler;
            animatiorHandler.AddActionTriggerListener(performAbility);
            
        }

        /// <summary>
        /// Change unsub to deactivate collider
        /// </summary>
        protected virtual void UnsubToPerformAction(Creature user, AbilityPerformAction performanceAction,
            Action performAbility) {
            //Remove Listener on animator trigger when done/Interrupted
            AnimatorHandler animatiorHandler = user.ActionHandler.AnimatorHandler;
            animatiorHandler.RemoveActionTriggerListener(performAbility);

        }
    }
}

