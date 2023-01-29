using RPG.Actions;
using RPG.Creatures;
using System;
using UnityEngine;

namespace RPG.Abilitys.Targeting {
    /// <summary>
    /// Uses a collider as a hit box to find targets
    /// </summary>
    [AttributeAbilityRequirements(AttributeAbilityRequirements.Gatekeep.Exclude)]
    public class TargetingHitbox : AbstractTargeting {
        public TargetingHitbox(int energy) : base(energy) {

            
        }

        //---TODO---
        //Get random position inside collider
        //Get random directions inside collider

        public override void FillPerformanceTargetingInfo(AbilityPerformAction performanceAction) {
            Func<Creature, Transform, (Vector3 forwardDirection, Vector3 upDirection)> getDirection = (user, equipment) => {


                (Vector3 upDirection, Ray ray, RaycastHit hit) rayInformation = user.GetRayLook();

                Vector3 hitPosition = rayInformation.hit.collider != null ?
                    rayInformation.hit.point : rayInformation.ray.GetPoint(30f);

                Vector3 aimDirection = hitPosition - equipment.position;
                return (aimDirection.normalized, rayInformation.upDirection);
            };

            performanceAction.SetDirectionFunction(getDirection);

            performanceAction.SetSubAction(SubToPerformAction);
            performanceAction.SetUnsubAction(UnsubToPerformAction);
            performanceAction.SetAnimationInfo(animationInfo);

        }

        public override GameObject[] TargetObject(Ability.AbilityBaseInfo abilityBaseInfo, Vector3 forwardDirection, Vector3 upDirection) {
            Debug.Log("HitBox: Cant be used in this way");
            
            return new GameObject[0];
        }

        public override Vector3[] TargetPosition(Ability.AbilityBaseInfo abilityBaseInfo, Vector3 forwardDirection, Vector3 upDirection) {
            Debug.Log("HitBox: Cant be used in this way");
            return new Vector3[0];
        }

        public override Vector3[] TargetDirection(Ability.AbilityBaseInfo abilityBaseInfo, Vector3 forwardDirection, Vector3 upDirection) {
            Debug.Log("HitBox: Cant be used in this way");
            return new Vector3[0];
        }

        /*---Protected---*/

        //Subscribes to a weapon script to pass GameoObjects hit to Form
        protected override void SubToPerformAction(Creature user, AbilityPerformAction performanceAction, Action performAbility) {
            
            //Sub to activate collider on equipment
            performanceAction.SubToActionActivate(performanceAction.targetingInfo.ActivateCollider);
            performanceAction.SubToActionDeactivate(performanceAction.targetingInfo.DeactivateCollider);


            performanceAction.SetTriggerAbility(PerformAbilityColliderTrigger);
            //Sub to call method when collider triggers
            performanceAction.SubToEquipmentColliderTrigger(performanceAction.PerformTriggerAbility);
        }
        protected override void UnsubToPerformAction(Creature user, AbilityPerformAction performanceAction, Action performAbility) {

            //Unsub to activate collider on equipment
            performanceAction.UnsubToActionActivate(performanceAction.targetingInfo.ActivateCollider);
            performanceAction.UnsubToActionDeactivate(performanceAction.targetingInfo.DeactivateCollider);

            //Encase of interruption
            performanceAction.targetingInfo.DeactivateCollider();

            //Unsub to call method when collider triggers
            performanceAction.UnsubToEquipmentColliderTrigger(performanceAction.PerformTriggerAbility);
        }

        /*---Private---*/

        //Perform this when collider hits something and check what info to pass on
        private void PerformAbilityColliderTrigger(Ability.AbilityBaseInfo abilityBaseInfo, 
            Vector3 startPosition, Vector3 forwardDirection, Vector3 upDirection, GameObject target) {

            Ability ability = abilityBaseInfo.ability;
            switch (ability.TargetingType) {
                default:
                case TargetType.Object:
                    ability.PerformAbilityAtFormCreature(abilityBaseInfo, startPosition, forwardDirection, upDirection, target);
                    break;
                case TargetType.Direction:
                    ability.PerformAbilityAtFormDirection(abilityBaseInfo, startPosition, forwardDirection, upDirection, 
                        ColliderTriggerDirection(startPosition, target));
                    break;
                case TargetType.Position:
                    ability.PerformAbilityAtFormPosition(abilityBaseInfo, startPosition, forwardDirection, upDirection, 
                        ColliderTriggerPosition(target));
                    break;
                 
            }
        }

        private Vector3 ColliderTriggerPosition(GameObject target) =>  target.GetComponent<Creature>().TargetMark.position;
        private Vector3 ColliderTriggerDirection(Vector3 startPosition, GameObject target) =>
            (target.GetComponent<Creature>().TargetMark.position - startPosition).normalized;


    }

}
