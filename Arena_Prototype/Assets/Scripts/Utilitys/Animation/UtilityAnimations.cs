namespace RPG.Creatures {
    /// <summary>
    /// Storage with Animation info
    /// </summary>
    public static class UtilityAnimations {


        public readonly static AnimationInfo Block = new("Block", 1);
        public readonly static AnimationInfo Death = new("Death", 0);
        public readonly static AnimationInfo Impact = new("Impact", 2);
        public readonly static AnimationInfo AttackSwing = new("AttackStarter", 1, new string[] { "AttackSwing" });
        public readonly static AnimationInfo AttackThrust = new("AttackStarter", 1, new string[] { "AttackThrust" });
        public readonly static AnimationInfo AttackUnarmed = new("AttackStarter", 1, new string[] { "AttackUnarmed" });
        public readonly static AnimationInfo AttackBow = new("AttackStarter", 1, new string[] { "AttackBow" });
        public readonly static AnimationInfo AttackThrustSpell = new("AttackStarter", 1, new string[] { "AttackThrustSpell" });
        public readonly static AnimationInfo AttackPowerup = new("AttackStarter", 1, new string[] { "AttackPowerup" });


        public struct AnimationInfo {
            public string stateName;
            public float normalizedTransitionDuration;
            public int layer;
            public string[] triggers;
            public (string, bool)[] boolens;

            public AnimationInfo(string stateName, int layer,
                string[] triggers = null, (string, bool)[] boolens = null, float normalizedTransitionDuration = 0.2f) {

                this.stateName = stateName;
                this.normalizedTransitionDuration = normalizedTransitionDuration;
                this.layer = layer;
                this.triggers = triggers != null ? triggers : new string[0];
                this.boolens = boolens != null ? boolens : new (string, bool)[0];
            }
        }
    }

}

