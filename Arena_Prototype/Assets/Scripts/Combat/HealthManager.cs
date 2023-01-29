using UnityEngine;
using RPG.Statistics;
using System;

namespace RPG.Creatures {
    public class HealthManager {

        ResultObserver<StatHealth, int> healthObserver;
        int currentHealth = 0;
        int currentMaxHealth = 0;

        //<currentHealth, MaxHealth>
        Action<int, int> healthAction;

        public HealthManager(GameObject user, IStatisticsController statisticsController) {

            healthObserver = new(statisticsController);
            currentMaxHealth = StatHealth.GetValue(statisticsController);
            currentHealth = currentMaxHealth;

            healthObserver.AddUpdateMethod(MaxHealthChange);
        }

        public void AddHealthListener(Action<int, int> healthAction) => this.healthAction += healthAction;
        public void RemoveHealthListener(Action<int, int> healthAction) => this.healthAction -= healthAction;
        /// <summary>
        /// Returns true if dead (health < 1)
        /// </summary> 
        /// <returns></returns>
        public bool TakeDamage(int damage) {

            currentHealth = (int)MathF.Max(currentHealth - damage, 0);
            UpdateHealth();
            return currentHealth < 1;
        }

        private void MaxHealthChange(int newMaxHealth) {

            //Change current health with the Max health changes
            //Prevent death of Max health change unless Max health drops to 0
            newMaxHealth = Mathf.Max(newMaxHealth, 0);
            currentHealth = Mathf.Max(currentHealth + (newMaxHealth - currentMaxHealth), 0);

            currentMaxHealth = newMaxHealth;
            currentHealth = Mathf.Min(currentHealth, currentMaxHealth);
            UpdateHealth();
        }

        private void UpdateHealth() {
            healthAction?.Invoke(currentHealth, currentMaxHealth);
        }
    }

}
