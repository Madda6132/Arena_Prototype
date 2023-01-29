using UnityEngine;
using UnityEngine.UI;
using RPG.Creatures;
using System.Threading.Tasks;

namespace RPG.UI {
    [RequireComponent(typeof(Creatures.UI.UILookAtCamera))]
    public class UIHeadHealthBar : MonoBehaviour {

        //Controls the UI facing direction 
        Creatures.UI.UILookAtCamera lookAtCamera;
        [SerializeField] Image bar;
        Creature creature;

        Task fadeTask;

        private void Start() {
            creature = transform.GetComponentInParent<Creature>();
            lookAtCamera = GetComponent<Creatures.UI.UILookAtCamera>();
            lookAtCamera?.Deactivate();
            creature?.SubToHealth(FillAmount);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Utilitys.FadeUI(0, 0, gameObject);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        /// <summary>
        /// Show Health bar and fade it
        /// </summary>
        public async void FillAmount(int currentHP, int maxHP) {

            if(0 < maxHP)
                bar.fillAmount = (float)currentHP / (float)maxHP;

            if (fadeTask != null && !fadeTask.IsCompleted) { 
                await Utilitys.FadeUI(0, 0, gameObject);
                await Task.Delay(1);
            }

            await Utilitys.FadeUI(1, 0, gameObject);
            lookAtCamera?.Activate();
            await Task.Delay(1000);
            fadeTask = Utilitys.FadeUI(0, 2f, gameObject);
            await fadeTask;
            lookAtCamera?.Deactivate();
        }
        

    }

}
