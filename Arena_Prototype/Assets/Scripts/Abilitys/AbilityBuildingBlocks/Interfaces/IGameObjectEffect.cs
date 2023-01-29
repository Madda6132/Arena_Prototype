using UnityEngine;

namespace RPG.Abilitys.Effect {
    /// <summary>
    /// Used on effects to sort it in FormBehavior for it to send out information to it
    /// </summary>
    public interface IGameObjectEffect {
        public void PerformEffectOnObjects(AbstractFormBehavior formBehavior, GameObject[] targets);
    }
}
