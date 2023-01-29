using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace RPG.Abilitys.Form {
    public class BehaviorProjectile : AbstractFormBehavior {

        [SerializeField] ParticleEffectTask[] _EndParticleEffect;

        [SerializeField] float _TravleSpeed = 1f;

        float explosionRadius = 2f;
        int pointsAmount = 3;

        public override void StartForm(Ability.AbilityBaseInfo abilityBaseInfo, AbstractForm form) {
            base.StartForm(abilityBaseInfo, form);

            AddDestroyOnTrigger();
        }

        public override GameObject[] GetTargetObjects() =>
            Physics.OverlapSphere(transform.position, explosionRadius).Select(x => x.gameObject).ToArray();

        public override Vector3[] GetTargetPositions() {
            Ray[] directions = UtilityForm.GetArcPointDirections(transform.position, transform.forward, transform.up,
                60f, pointsAmount);
            return directions.Select(x => x.GetPoint(1f)).ToArray();
        }

        /*---Protected---*/

        protected override Vector3 GetStartPosition() => transform.position;
        protected override Vector3 GetEndPosition() => transform.position;
        protected override Vector3 GetTickPosition() => transform.position;

        protected override void ExtraUpdate() {

            if(!_IsLifeIsEnding)
                transform.position += transform.forward * Utilitys.TRAVEL_SPEED * _TravleSpeed * Time.deltaTime;
        }


        protected override void EndFormBehavior() {

            //Create a Particle effect on Destroy
            foreach (var particleEffect in _EndParticleEffect) {
                particleEffect.CreateTemporarilyEffect(this);
            }

            base.EndFormBehavior();
        }

        /*---Private---*/

        private void OnTriggerEnter(Collider other) => SendGameObject(other.gameObject);


        private void AddDestroyOnTrigger() {
            //Will be destroyed on impact
            OnTriggeredForm += (form, target) => StartingEndingFormBehavior();
        }
    }

}
