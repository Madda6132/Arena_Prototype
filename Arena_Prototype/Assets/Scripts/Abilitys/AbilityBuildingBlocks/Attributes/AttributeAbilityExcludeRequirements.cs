using System;

namespace RPG.Abilitys {
    
    public class AttributeAbilityExcludeRequirements : AbstractAttributeAbilityRequirements {
        public AttributeAbilityExcludeRequirements(AbilityElement[] elements = null, Type[] abstractForms = null,
            Type[] abstractTargeting = null) : base(elements, abstractForms, abstractTargeting) {


            //To exclude or include
            _IsIncluding = false;

        }
    }
}