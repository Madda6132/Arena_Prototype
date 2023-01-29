using RPG.Creatures;
using UnityEngine;

namespace RPG.Abilitys.Form {
    /// <summary>
    /// Create Projectile form
    /// </summary>
    public class FormProjectile : AbstractForm {
        public FormProjectile(int energy) : base(energy) {

        }


        public override AbstractFormBehavior StartFormCreature(Ability.AbilityBaseInfo abilityBaseInfo,
            Vector3 startPosition, Vector3 forwardDirection, Vector3 upDirection, GameObject creature) {
            Debug.Log("Projectile created: Targeting creature");
            Vector3 direction = creature.TryGetComponent(out IDamageable _Damageable) ? (_Damageable.TargetMark.position - startPosition).normalized : (creature.transform.position - startPosition).normalized;

            BehaviorProjectile projectileObject = CreateForm<BehaviorProjectile>(abilityBaseInfo, startPosition, direction);
            projectileObject.StartForm(abilityBaseInfo, this);
            return projectileObject; 
        }

        public override AbstractFormBehavior StartFromDirection(Ability.AbilityBaseInfo abilityBaseInfo,
            Vector3 startPosition, Vector3 forwardDirection, Vector3 upDirection, Vector3 direction) {
            Debug.Log("Projectile created: Targeting position");
            BehaviorProjectile projectileObject = CreateForm<BehaviorProjectile>(abilityBaseInfo, startPosition, direction);
            projectileObject.StartForm(abilityBaseInfo, this);
            return projectileObject;
        }

        public override AbstractFormBehavior StartFromPosition(Ability.AbilityBaseInfo abilityBaseInfo,
            Vector3 startPosition, Vector3 forwardDirection, Vector3 upDirection, Vector3 position) {
            Debug.Log("Projectile created: Targeting direction");
            BehaviorProjectile projectileObject = CreateForm<BehaviorProjectile>(abilityBaseInfo, startPosition, 
                (startPosition - position).normalized);

            projectileObject.StartForm(abilityBaseInfo, this);
            return projectileObject; 
        }

        protected override void SetTargetTypeAndCost() {
            TargetType[] randomTargetList = new TargetType[] { TargetType.Object, TargetType.Position, TargetType.Direction };
            targetingType = randomTargetList[Random.Range(1, randomTargetList.Length)];

            procentAddedCost = 0.2f;
        }
        

    }

}
