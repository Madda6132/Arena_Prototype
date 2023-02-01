using UnityEngine;
using System.Linq;

namespace RPG.Abilitys.Targeting {
    public class TargetingEmission : AbstractTargeting {

        int _EmissionRadius = 5;
        int _EmissionPoints = 3;

        public TargetingEmission(int energy) : base(energy) {


            animationInfo = Creatures.UtilityAnimations.AttackPowerup;

            int _Multiplier = 600 < energy ? energy / 300 : 1;

            _EmissionRadius *= _Multiplier;
            _EmissionPoints *= _Multiplier;

        }


        public override Vector3[] TargetDirection(Ability.AbilityBaseInfo abilityBaseInfo, Vector3 forwardDirection, Vector3 upDirection) {
            
            Ray[] rays = UtilityForm.GetCircleDirections(abilityBaseInfo.startPosition, forwardDirection, upDirection, _EmissionPoints);
            
            return rays.Select(x => x.direction).ToArray();
        }

        public override GameObject[] TargetObject(Ability.AbilityBaseInfo abilityBaseInfo, Vector3 forwardDirection, Vector3 upDirection) {
            
            int layerIndex = Utilitys.LayerMaskBitIndex(LayerMask.NameToLayer("TargetbleObject"));
            Collider[] colliders = Physics.OverlapSphere(abilityBaseInfo.startPosition, _EmissionRadius, layerIndex);

            return colliders.Select(x => x.gameObject).ToArray();
        }

        public override Vector3[] TargetPosition(Ability.AbilityBaseInfo abilityBaseInfo, Vector3 forwardDirection, Vector3 upDirection) {
            
            return UtilityForm.GetCircleRandomPoints(abilityBaseInfo.startPosition, forwardDirection, upDirection, 
                _EmissionRadius, _EmissionPoints);
        }
    }
}

