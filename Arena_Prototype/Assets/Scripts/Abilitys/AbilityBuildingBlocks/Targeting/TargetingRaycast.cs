using UnityEngine;
using System.Linq;

namespace RPG.Abilitys.Targeting {
    /// <summary>
    /// Uses a position as a start point and sends rays in a direction to find targets
    /// </summary>
    public class TargetingRaycast : AbstractTargeting {

        
        public float raySphereSize = 0.1f;
        public int rayTargetAmount = 1;

        
        const float MAX_RAYLENGTH = 30f;

        public TargetingRaycast(int energy) : base(energy) {

            //animationInfo = Creatures.UtilityAnimations.AttackThrustSpell;
            
            int _Multiplier = 200 < energy ? energy / 100 : 1;

            raySphereSize *= _Multiplier;
            rayTargetAmount *= _Multiplier;
            //---TODO---
            //Use energy to determine the size and target amount of the ray

        }


        //Ray/Sphere-Cast all in a line 
        public override GameObject[] TargetObject(Ability.AbilityBaseInfo abilityBaseInfo, Vector3 forwardDirection,
            Vector3 upDirection) {

            GameObject[] targets = GetSphereCastHits(abilityBaseInfo, forwardDirection).
                        Select(x => x.transform.gameObject).ToArray();

            return targets;
        }

        //Get points in an arc and sends Rays towards those directions
        public override Vector3[] TargetPosition(Ability.AbilityBaseInfo abilityBaseInfo, Vector3 forwardDirection, 
            Vector3 upDirection) {

            Ray[] rays = UtilityForm.GetArcPointDirections(abilityBaseInfo.startPosition, forwardDirection, upDirection,
                45f, rayTargetAmount);

            Vector3[] targets = new Vector3[rays.Length];

            for (int i = 0; i < targets.Length; i++) {

                RaycastHit hit;
                if(Physics.Raycast(rays[i], out hit, MAX_RAYLENGTH)) {
                    targets[i] = hit.point;
                } else {
                    targets[i] = rays[i].GetPoint(MAX_RAYLENGTH);
                }
            }

            return targets;
        }

        //Get points in an arc as directions
        public override Vector3[] TargetDirection(Ability.AbilityBaseInfo abilityBaseInfo, Vector3 forwardDirection, 
            Vector3 upDirection) {
            
            Ray[] rays = UtilityForm.GetArcPointDirections(abilityBaseInfo.startPosition, forwardDirection, upDirection,
                45f, rayTargetAmount);
            
            return rays.Select(x => x.direction).ToArray();
        }

        /*---Private---*/

        private RaycastHit[] GetSphereCastHits(Ability.AbilityBaseInfo abilityBaseInfo, Vector3 forwardDirection) {

            int layerIndex = Utilitys.LayerMaskBitIndex(LayerMask.NameToLayer("TargetbleObject"));
            RaycastHit[] hits;
            hits = Physics.SphereCastAll(abilityBaseInfo.startPosition, raySphereSize, forwardDirection, 30f, layerIndex);


            if (hits == null) hits = new RaycastHit[0];

            return hits = hits.OrderByDescending(x => x.distance).Take(rayTargetAmount).ToArray();
        }
    }
}
