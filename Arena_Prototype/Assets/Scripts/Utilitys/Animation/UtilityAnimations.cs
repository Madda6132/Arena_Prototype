using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace RPG.Creatures {
    /// <summary>
    /// Storage with Animation info
    /// </summary>
    public static class UtilityAnimations {

        public enum AnimatorMessageType {
            ACTION_ACTIVE,
            ACTION_DEACTIVE,
            ACTION_END,
            ACTION_CHANNEL,
            ACTION_REPEAT,
            ACTION_TRIGGER
        }

        public readonly static int UPPERBODY_INDEX_LAYER = LayerMask.NameToLayer("TargetbleObject");

        public static AnimationInfo Block { get => new("Block", 1); }
        public static AnimationInfo Death { get => new("Death", 0); }
        public static AnimationInfo Impact { get => new("Impact", 2); }
        public static AnimationInfo AttackSwing { get => new("Swing_Start", 1);}
        public static AnimationInfo AttackThrust { get => new("Thrust_start", 1);}
        public static AnimationInfo AttackUnarmed { get => new("Unarmed_Start", 1);}
        public static AnimationInfo AttackBow { get => new("Box_Start", 1);}
        public static AnimationInfo AttackThrustSpell { get => new("ThrustSpell_Start", 1);}
        public static AnimationInfo AttackPowerup { get => new("Powerup_Start", 1);}

        public class AnimationInfo {
            public string stateName;
            public float normalizedTransitionDuration;
            public int layer;
            public List<string> triggers;
            public List<(string, bool)> boolens;

            public AnimationInfo(string stateName, int layer,
                string[] triggers = null, (string, bool)[] boolens = null, float normalizedTransitionDuration = 0.2f) {

                this.stateName = stateName;
                this.normalizedTransitionDuration = normalizedTransitionDuration;
                this.layer = layer;
                this.triggers = triggers != null ? triggers.ToList() : new string[0].ToList();
                this.boolens = boolens != null ? boolens.ToList() : new (string, bool)[0].ToList();
            }
        }
    }

}

