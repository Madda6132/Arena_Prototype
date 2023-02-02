using UnityEngine;

namespace RPG.Abilitys.Form {
    /// <summary>
    /// Create Explosion form
    /// </summary>
    public class FormExplosion : AbstractForm {
        public FormExplosion(int energy) : base(energy) {

        }

        public override AbstractFormBehavior StartFormCreature(Ability.AbilityBaseInfo abilityBaseInfo,
            Vector3 startPosition, Vector3 forwardDirection, Vector3 upDirection, GameObject creature) {

            BehaviorExplosion explosionObject = GetObjectBehavior<BehaviorExplosion>(abilityBaseInfo, startPosition, forwardDirection);
            explosionObject.StartForm(abilityBaseInfo);
            return explosionObject;
        }

        public override AbstractFormBehavior StartFromDirection(Ability.AbilityBaseInfo abilityBaseInfo,
            Vector3 startPosition, Vector3 forwardDirection, Vector3 upDirection, Vector3 direction) {
            
            BehaviorExplosion explosionObject = GetObjectBehavior<BehaviorExplosion>(abilityBaseInfo, startPosition, forwardDirection); ;
            explosionObject.StartForm(abilityBaseInfo);
            return explosionObject;
        }

        public override AbstractFormBehavior StartFromPosition(Ability.AbilityBaseInfo abilityBaseInfo, 
            Vector3 startPosition, Vector3 forwardDirection, Vector3 upDirection, Vector3 position) {
            
            BehaviorExplosion explosionObject = GetObjectBehavior<BehaviorExplosion>(abilityBaseInfo, position, forwardDirection);
            explosionObject.StartForm(abilityBaseInfo);
            return explosionObject;
        }

        /*---Protected---*/

        protected override void SetTargetTypeAndCost() {

            //TargetType[] randomTargetList = new TargetType[] { TargetType.Object, TargetType.Position, TargetType.Direction };
            //targetingType = randomTargetList[UnityEngine.Random.Range(1, randomTargetList.Length)];

            procentAddedCost = 0.2f;
        }

        

    }

}
