using RPG.Creatures;
using UnityEngine;
using RPG.Abilitys.Form;

namespace RPG.Abilitys.Effect {
    /// <summary>
    /// Deal damage to creatures
    /// </summary>
    public class EffectDamage : AbstractAbilityEffect, IGameObjectEffect {

        //When created
        //Set energy scale 0.2 - 5 
        //The energy scale will depend  on the cost

        int damage = 1; 


        public EffectDamage(int energy):base(energy) {

            float energyScale = Random.Range(2f, 11f)/10;

            damage = (int)(energyScale * energy);
            
        }

        public void PerformEffectOnObjects(AbstractFormBehavior formBehavior, GameObject[] targets) {

            foreach (var target in targets) {

                if (target.TryGetComponent(out Creature creature))
                 creature.TakeDamage(damage);
            }
        }
    }

}
