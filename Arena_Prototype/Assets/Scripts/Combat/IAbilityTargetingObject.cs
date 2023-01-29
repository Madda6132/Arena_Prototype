using UnityEngine;
using System;

namespace RPG.Combat {
    /// <summary>
    /// Provide the information needed for targeting. Primarily for AbilityBuildingBlock_Targeting.
    /// </summary>
    public interface IAbilityTargetingObject {

        /// <summary>
        /// Example: Hit box collider
        /// </summary>
        public Collider TargetingCollider { get; set; }
        /// <summary>
        /// Targeting point. Example: provide raycast input
        /// </summary>
        public Transform TargetingTransform { get; set; }

        public void ActivateCollider();
        public void DeactivateCollider();

        /// <summary>
        /// Add to listen to a collider when it is triggered
        /// </summary>
        /// <param name="Lister"> The action that will revive the triggered target </param>
        public void SubToColliderTrigger(Action<GameObject> Lister);
        /// <summary>
        /// Remove from listen to a collider when it is triggered
        /// </summary>
        /// <param name="Lister"> The action that will revive the triggered target </param>
        public void UnsubToColliderTrigger(Action<GameObject> Lister);

    }

}
