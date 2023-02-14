using UnityEngine;
using RPG.Abilitys.Form;

namespace RPG.Abilitys.Effect {
    /// <summary>
    /// Used on effects to sort it in FormBehavior for it to send out information to it
    /// </summary>
    public interface IDirectionEffect {
        public void PerformEffectOnDirection(AbstractFormBehavior formBehavior, Vector3[] directions);
    }
}

