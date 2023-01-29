using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Creatures {
    /// <summary>
    /// Tells the combat scripts that this can be damaged
    /// </summary>
    public interface IDamageable {

        public void TakeDamage(int damage);

        //This will prevent targets to aim at the gameobjects base e.g feet
        /// <summary>
        /// A specific point on the gameobject that combat will target
        /// </summary>
        public Transform TargetMark { get; }

        //Currently not implemented.
        /// <summary>
        /// Will test targets defense before causing its effects on it
        /// </summary>
        public void TakeCombatEffect(Combat.EffectInformation effectInformation);
    }
}

