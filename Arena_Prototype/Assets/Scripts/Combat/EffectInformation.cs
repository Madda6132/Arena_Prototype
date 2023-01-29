using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Creatures;
using System;

namespace RPG.Combat { 
    //The idea is to use this to
    //Confirm Hit against "Defense"
    //Input damage/Heal type, the amount and against what defense
    public class EffectInformation 
    {


        public Func<Creature, bool> checkIfHit { 
            get { 
                return checkIfHit; 
            } 
            set {
                if (value != null) checkIfHit = value;
            } 
        }

        private List<Abilitys.Effect.AbstractAbilityEffect> effects;

        public EffectInformation(Abilitys.Effect.AbstractAbilityEffect[] effects, Func<Creature, bool> checkHit = null) {


            checkIfHit = checkHit != null ? checkHit : DefaultCheckHit;
        }

        /*---Private---*/

        //Damage type "Fire/Lighting"
        //Damage Amount
        //Instigator "user"
        //Attack to hit? (Func<Creatre, bool>)

        private bool DefaultCheckHit(Creature creature) {

            return creature != null;
        }
    }
}
