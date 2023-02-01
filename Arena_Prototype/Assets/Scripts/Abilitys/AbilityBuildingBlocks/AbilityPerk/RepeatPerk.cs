using UnityEngine;

namespace RPG.Abilitys.Perk {

    /// <summary>
    /// Repeat an ability's performance at the FormBehavior's location a set amount of times
    /// </summary>
    [AttributeAbilityExcludeRequirements( abstractTargeting: new System.Type[] {typeof(Targeting.TargetingHitbox) })]
    public class RepeatPerk : AbstractAbilityPerk {

        public int repeatAmountCount { get; private set; } = 1;

        public RepeatPerk(int energy) : base(energy) {

            repeatAmountCount = Random.Range(1, 5);
        }

        public override object GetPerkStorage(AbstractFormBehavior formBehavior) =>
            new RepeatStorage(this, repeatAmountCount);

        public void Perform(RepeatStorage storage, AbstractFormBehavior behavior) {

            AbstractFormBehavior[] newFormBehaviors = behavior.Repeat();

            foreach (var formBehavior in newFormBehaviors) {
                formBehavior.ReplaceStorage(storage.Clone());
            }
            
        }


        public class RepeatStorage : IActionMessageListener {

            RepeatPerk perk;
            public int repeatCount = 1;

            public RepeatStorage(RepeatPerk perk, int repeatCount) { 
                
                this.perk = perk;
                this.repeatCount = repeatCount;
            }


            public void Perform(AbstractFormBehavior behavior, string message) {

                if (0 < repeatCount && message == Form.FormUtilityMessages.END) {

                    repeatCount--;
                    perk.Perform(this, behavior);
                }
            }

            public RepeatStorage Clone() => new(perk, repeatCount);
        }
    }

}
