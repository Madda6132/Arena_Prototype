using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Creatures;
using RPG.Actions;
using RPG.Combat;

namespace RPG.Abilitys {
    /// <summary>
    /// A power-up or channeling ability. 
    /// Not started yet
    /// </summary>
    public class ChannelAbility {

        Ability ability;

        public ChannelAbility(AbilityElement abilityElement, int energy) {

            //Equipment sends element, energy
            Abilitys.Form.FormProjectile form = new(energy);
            Perk.AbstractAbilityPerk[] repPerk = { new Perk.RepeatPerk(energy) };
            ability = new(energy, abilityElement, TargetType.Direction, targeting: new Targeting.TargetingRaycast(energy), form: form, perk: repPerk);
        }

        public IPerformAction GetAbilityAction(Creature creature, IAbilityTargetingObject targetingInfo) =>
            ability.GetChannelAction(creature, targetingInfo);



    }

}
