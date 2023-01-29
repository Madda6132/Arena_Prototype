using UnityEngine;

namespace RPG.Abilitys.Effect {
    /// <summary>
    /// Used on effects to sort it in FormBehavior for it to send out information to it
    /// </summary>
    public interface IPositionEffect {

        public void PerformEffectOnPosition(AbstractFormBehavior formBehavior, Vector3[] targets);
    }
}
