using System;

namespace RPG.Abilitys {

    /// <summary>
    /// Gatekeeper Ability from Ability building blocks by the ability's element (Fire/Ice/Lightning) and/or its form and/or its targeting
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public abstract class AbstractAttributeAbilityRequirements : Attribute
    {
        protected bool _IsIncluding;

        protected AbilityElement[] _Elements;
        protected Type[] _Forms;
        protected Type[] _Targeting;
        

        public AbstractAttributeAbilityRequirements(AbilityElement[] elements = default, 
            Type[] abstractForms = default, Type[] abstractTargeting = default) {


            //Store Element types
            this._Elements = elements != null ? elements : new AbilityElement[0];

            //Store Form
            this._Forms = abstractForms != null ? abstractForms : new Type[0];


            //Store Targeting
            this._Targeting = abstractTargeting != null ? abstractTargeting : new Type[0];

        }

        public bool CheckRequirements(Ability ability) {


            //If this it true. It's only accessible through direct creation
            
            if (_Elements.Length == 0 && _Forms.Length == 0 && _Targeting.Length == 0) return false;

            bool allow = true;

            
            foreach (var element in _Elements) {
                allow = ability.GetElement == element ? _IsIncluding : !_IsIncluding;
                if (!allow) return allow;
            }

            foreach (var form in _Forms) {
                Form.AbstractForm abstractForm = ability.GetForm;
                allow = abstractForm != null && abstractForm.GetType() == form ? _IsIncluding : !_IsIncluding;
                if (!allow) return allow;
            }

            foreach (var target in _Targeting) {
                Targeting.AbstractTargeting abstractTargeting = ability.GetTargeting;
                allow = abstractTargeting != null && abstractTargeting.GetType() == target ? _IsIncluding : !_IsIncluding;
                if (!allow) return allow;
            }


            return allow;
        }
    }
}