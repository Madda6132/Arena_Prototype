using UnityEngine;

namespace RPG.Abilitys.Perk {
    /// <summary>
    /// Prevents FormBehavior from destroying itself a amount of times
    /// </summary>
    [AttributeAbilityIncludeRequirements(abstractForms: new System.Type[] {typeof(Form.FormProjectile)})]
    //Projectile will be prevented from being destroyed after colliding with a creature
    public class PenetratPerk : AbstractAbilityPerk {

        public int penetratAmountCount { get; private set; } = 1;

        public PenetratPerk(int energy) : base(energy) {

            //-1 means infinite
            int[] penetraitAmount = new int[] {-1, 1, 2, 3, 4, 5 };
            penetratAmountCount = penetraitAmount[Random.Range(0, penetraitAmount.Length)];
        }

        public override object GetPerkStorage(AbstractFormBehavior formBehavior) =>
            new PenetratStorage(this);
         
        public void Perform(AbstractFormBehavior behavior) =>
            behavior.InteruptAction();

        

        public class PenetratStorage : IActionMessageListener {

            PenetratPerk perk;
            int penetratAmountCount = 1;

            public PenetratStorage(PenetratPerk perk) {
                this.perk = perk;
                penetratAmountCount = perk.penetratAmountCount;
            }

            public void Perform(AbstractFormBehavior behavior, string message) {

                if (penetratAmountCount != 0 && message == Form.FormUtilityMessages.START_DESTROY) {

                    if(penetratAmountCount != 0) penetratAmountCount--;

                    perk.Perform(behavior);
                }
                
            }
        }
    }
}