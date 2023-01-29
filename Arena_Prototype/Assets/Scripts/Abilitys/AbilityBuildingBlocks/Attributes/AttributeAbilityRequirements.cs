using System; 

namespace RPG.Abilitys {

    /// <summary>
    /// Gatekeeper Ability from Ability building blocks by the ability's element (Fire/Ice/Lightning) and/or its form 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class AttributeAbilityRequirements : Attribute
    {
        AbilityElement[] elements;
        Type[] forms;
        bool isExcluding;
        public enum Gatekeep {
            Include,
            Exclude
        }

        public AttributeAbilityRequirements(Gatekeep gatekeeping, AbilityElement[] elements = default, Type[] abstractForms = default) {

            //To exclude or include
            isExcluding = gatekeeping == Gatekeep.Exclude ? true : false;

            //Store Element types
            this.elements = elements != null ? elements : new AbilityElement[0];

            //Store Form
            this.forms = abstractForms != null ? abstractForms : new Type[0];

        }

        public bool CheckRequirements(Ability ability) {

            
            //If this it true. It's only accessible through direct creation
            if(elements.Length == 0 && forms.Length == 0) return false;

            bool allow = true;

            
            foreach (var element in elements) {
                allow = ability.GetElement == element ? isExcluding : !isExcluding;
                if (!allow) return allow;
            }
            foreach (var form in forms) {
                Form.AbstractForm abstractForm = ability.GetForm;
                allow = abstractForm != null && abstractForm.GetType() == form ? isExcluding : !isExcluding;
                if (!allow) return allow;
            }
            
            
            return allow;
        }
    }
}