using UnityEngine;
using RPG.Abilitys;
using RPG.Creatures;
using RPG.Actions;
using RPG.Combat;
using System;

namespace RPG.Inventory {
    /// <summary>
    /// Currently only weapons that holds quickAbility and ChannelAbility
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class Equipment : MonoBehaviour, IAbilityTargetingObject {

        public Collider TargetingCollider { get => _Collider; set => _Collider = value; }
        public Transform TargetingTransform { get => AbilityStartLocation; set => AbilityStartLocation = value; }

        /// <summary>
        /// Used when targeting from ability
        /// </summary>
        [SerializeField] private Transform AbilityStartLocation;

        Collider _Collider;
        Action<GameObject> OnTargetEnterTrigger;

        //---TODO---
        //Animator overrider for the type of weapon used
        
        //Abilitys click and hold
        QuickAbility quickAbility;
        ChannelAbility channelAbility;

        public Transform GetAbilityStartLocation => AbilityStartLocation;
        public QuickAbility GetQuickAbility => quickAbility;
        public ChannelAbility GetChannelAbility => channelAbility;

        public IPerformAction GetAbilityPerformAction(Creature creature) => quickAbility.GetAbilityAction(creature, this);

        public IPerformAction ActivateChannelAbility(Creature creature) => channelAbility.GetAbilityAction(creature, this);

        public void ActivateCollider() => TargetingCollider.enabled = true;
        public void DeactivateCollider() => TargetingCollider.enabled = false;

        public void SubToColliderTrigger(Action<GameObject> Lister) => OnTargetEnterTrigger += Lister;
        public void UnsubToColliderTrigger(Action<GameObject> Lister) => OnTargetEnterTrigger -= Lister;

        /*---Private---*/

        private void OnDrawGizmosSelected() {

            Gizmos.DrawSphere(transform.position, 0.05f);
        }

        private void Awake() {
            //Can be chanced to a SerializeField object instead of getting a collider on the object
            TargetingCollider = GetComponent<Collider>();
            
            //Use this when setting equipment
            Collider userCollider = GetComponentInParent<Creature>().GetComponent<Collider>();
            Physics.IgnoreCollision(TargetingCollider, userCollider, true);

            quickAbility = new(AbilityElement.Fire, 400);
            channelAbility = new(AbilityElement.Fire, 400);
        }

        private void OnTriggerEnter(Collider other) {

            if(other.TryGetComponent(out IDamageable creature))
                OnTargetEnterTrigger?.Invoke(other.gameObject);
        }

    }
}

