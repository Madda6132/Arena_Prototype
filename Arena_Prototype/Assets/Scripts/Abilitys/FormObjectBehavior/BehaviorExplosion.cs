using RPG.Abilitys.Form;
using UnityEngine;
using System.Linq;

namespace RPG.Abilitys.Form {
        public class BehaviorExplosion : AbstractFormBehavior {

    
        float explosionRadius = 2f;
        int pointsAmount = 3;


        public override void StartForm(RPG.Abilitys.Ability.AbilityBaseInfo abilityBaseInfo) {
            base.StartForm(abilityBaseInfo);


            GameObject[] _Targets = Physics.OverlapSphere(transform.position, explosionRadius).Select(x => 
                x.gameObject).ToArray();

            SendGameObject(_Targets);
        }

        public override Vector3[] GetTargetPositions() =>
            RPG.UtilityForm.GetCircleRandomPoints(transform, explosionRadius, pointsAmount);


        public override GameObject[] GetTargetObjects() =>
            Physics.OverlapSphere(transform.position, explosionRadius).Select(x => x.gameObject).ToArray();

        /*---Protected---*/

        protected override Vector3 GetStartPosition() => transform.position;
        protected override Vector3 GetEndPosition() => transform.position;
        protected override Vector3 GetTickPosition() => transform.position;
        protected override void ExtraUpdate() { }

        /*---Private---*/

        private void Start() {
            _LifeTime = 2f;

        }
    }
}
