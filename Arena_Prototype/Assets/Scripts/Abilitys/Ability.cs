using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System;
using RPG.Creatures;

namespace RPG.Abilitys {

    public enum TargetType {
        NONE,
        Object,
        Direction,
        Position
    }
    public enum AbilityElement {
        Fire,
        Frost,
        Lightning

    }
    /// <summary>
    /// Ability is a framework consists of Targeting, Form and Effect. 
    /// Additionally can contain subAbilitys that are abilitys, and perks that are performed as certain events.
    /// </summary>
    public class Ability : IAbility{

        const int MILESTONE_SUBABILITY = 250;
        const int MILESTONE_PERK = 400;

        float _AbilityRange = 1f;

        //Main
        Targeting.AbstractTargeting _Targeting;
        Form.AbstractForm _Form;
        Effect.AbstractAbilityEffect[] _Effects;

        //Minor
        List<SubAbility> _SubAbilitys = new();
        List<Perk.AbstractAbilityPerk> _AbilityPerks = new();


        //What type of targeting the abstractTargeting should get
        public TargetType _TargetingType { get; private set; } = TargetType.Object;

        //Determines the options the ability has when generating random Targeting/Form/Effects
        AbilityElement _AbilityElement;
        int _AbilityEnergyCost = 0;


        //---!TODO!---
        //Requirements // Are only for the first targeting and class? Ex No channels
        //Get Energy cost


        
        public Ability(int energy, AbilityElement element, TargetType TargetingType = TargetType.NONE, 
            Targeting.AbstractTargeting targeting = null, Form.AbstractForm form = null, Effect.AbstractAbilityEffect[] effects = null,
            Perk.AbstractAbilityPerk[] perk = null, SubAbility[] subAbility = null) {

            _AbilityEnergyCost = energy;
            _AbilityElement = element;


            _TargetingType = TargetingType != TargetType.NONE ? TargetingType : GenerateTargetType();
            this._Targeting = targeting != null ? targeting : GenerateTargeting();
            this._Form = form != null ? form : GenerateForm();
            this._Effects = effects != null ? effects : GenerateEffects();

            _AbilityPerks = perk != null ? perk.ToList() : GeneratePerks().ToList();
            _SubAbilitys = subAbility != null ? subAbility.ToList() : GenerateSubAbility().ToList();
        }


        //Ability Range 
        public float GetRange(int energyCost) { 
            
            //Input energy into Targeting and Form to return range
            //Should start with getting range from targeting
            return _AbilityRange; 
        }

        //Cost //Allowed to iterate through all layers and cumulate cost
        public int GetCost() {

            int energyCost = _AbilityEnergyCost;

            foreach (var subAbility in _SubAbilitys) {
                //energyCost += subAbility.GetCost();
            }

            return energyCost; 
        }


        public void AddSubAbilities(SubAbility state) => _SubAbilitys.Add(state);
        public void AddAbilityPerk(Perk.AbstractAbilityPerk perk) => _AbilityPerks.Add(perk);

        public SubAbility[] GetSubAbilitys => _SubAbilitys.ToArray();
        public Perk.AbstractAbilityPerk[] GetPerks => _AbilityPerks.ToArray();
        public Targeting.AbstractTargeting GetTargeting => _Targeting;
        public Form.AbstractForm GetForm => _Form;
        public Effect.AbstractAbilityEffect[] GetEffects() => _Effects;
        public AbilityElement GetElement => _AbilityElement;

        /// <summary>
        /// Perform Ability from the start. Usually used during a attack trigger.
        /// </summary>
        /// <param name="abilityBaseInfo"> Information about startPoint, the user Creature, and energy </param>
        /// <param name="forwardDirection"> Usually, the direction the creature is facing. </param>
        /// <param name="upDirection"> The up direction for the ability.  </param>
        public AbstractFormBehavior[] PerformAbilityAtTargeting(AbilityBaseInfo abilityBaseInfo, Vector3 startPosition, 
            Vector3 forwardDirection, Vector3 upDirection) {

            Debug.DrawRay(abilityBaseInfo.startPosition, forwardDirection, Color.red, 5f);

            List<AbstractFormBehavior> _BehaviorList = new List<AbstractFormBehavior>();

            //Check forms targeting Preference
            //Perform Targeting
            //Perform form towards each target
            switch (_TargetingType) {
                default:
                case TargetType.Object:
                    foreach (var target in _Targeting.TargetObject(abilityBaseInfo, forwardDirection, upDirection)) {
                        _BehaviorList.Add(PerformAbilityAtFormCreature(abilityBaseInfo, startPosition, forwardDirection, upDirection, target));
                    }
                    break;
                case TargetType.Direction:
                    foreach (var target in _Targeting.TargetDirection(abilityBaseInfo, forwardDirection, upDirection)) {
                        _BehaviorList.Add(PerformAbilityAtFormDirection(abilityBaseInfo, startPosition, forwardDirection, upDirection, target));
                    }
                    break;
                case TargetType.Position:
                    foreach (var target in _Targeting.TargetPosition(abilityBaseInfo, forwardDirection, upDirection)) {
                        _BehaviorList.Add(PerformAbilityAtFormPosition(abilityBaseInfo, startPosition, forwardDirection, upDirection, target));
                    }
                    break;
                
            }

            return _BehaviorList.ToArray();

        }

        //Passing the Targeting element by passing the targets directly to form
        public AbstractFormBehavior PerformAbilityAtFormCreature(AbilityBaseInfo abilityBaseInfo, Vector3 startPosition, Vector3 forwardDirection, 
            Vector3 upDirection, GameObject targetCreature) => 
              _Form.StartFormCreature(abilityBaseInfo, startPosition, forwardDirection, upDirection, targetCreature);
        public AbstractFormBehavior PerformAbilityAtFormPosition(AbilityBaseInfo abilityBaseInfo, Vector3 startPosition, Vector3 forwardDirection,
            Vector3 upDirection, Vector3 position) => 
            _Form.StartFromPosition(abilityBaseInfo, startPosition, forwardDirection, upDirection, position);
        public AbstractFormBehavior PerformAbilityAtFormDirection(AbilityBaseInfo abilityBaseInfo, Vector3 startPosition, Vector3 forwardDirection,
            Vector3 upDirection, Vector3 dir) => 
            _Form.StartFromDirection(abilityBaseInfo, startPosition, forwardDirection, upDirection, dir);

        public Actions.AbilityPerformAction GetAbilityAction(Creature creature, Combat.IAbilityTargetingObject targetingInfo) =>
           new (this, creature, targetingInfo);

        public struct AbilityBaseInfo {

            public int energy { get; private set; }
            public Creature user { get; private set; }
            public Vector3 startPosition { get; private set; }
            public Ability ability { get; private set; }

            public AbilityBaseInfo(Ability ability, int energy, Creature user, Vector3 startPosition) {

                this.ability = ability;
                this.energy = energy;
                this.user = user;
                this.startPosition = startPosition;
            }
        }

        /*---Private---*/

        #region Ability Creation

        private TargetType GenerateTargetType() {

            int enumIndex = UnityEngine.Random.Range(1, Enum.GetValues(typeof(TargetType)).Length);

            return (TargetType)enumIndex;
        }

        private IEnumerable<Perk.AbstractAbilityPerk> GeneratePerks() {

            int amountOfperk = _AbilityEnergyCost / MILESTONE_PERK;
            for (int i = 0; i < amountOfperk; i++) {

                Perk.AbstractAbilityPerk perk = GetRandomPerk();

                if(perk != null) yield return perk;

            }
        }

        private IEnumerable<SubAbility> GenerateSubAbility() {


            int amountOfSubAbility = _AbilityEnergyCost / MILESTONE_SUBABILITY;
            for (int i = 0; i < amountOfSubAbility; i++) {

                yield return GetRandomSubAbility(_AbilityEnergyCost);

            }
        }

        private Form.AbstractForm GenerateForm() {

            //Assembly gets all available types from it that is classes and from the namespace
            var list = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.BaseType != null &&
            x.BaseType == typeof(Form.AbstractForm)).ToList();

            Form.AbstractForm form = CreateRandomAllowedType<Form.AbstractForm>(list, _AbilityEnergyCost);

            return form;
        }
        private Targeting.AbstractTargeting GenerateTargeting(){

            //Assembly gets all available types from it that is classes and from the namespace
            var list = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.BaseType != null &&
            x.BaseType == typeof(Targeting.AbstractTargeting)).ToList();

            Targeting.AbstractTargeting targeting = CreateRandomAllowedType<Targeting.AbstractTargeting>(list, _AbilityEnergyCost);

            return targeting; 
        }
        private Effect.AbstractAbilityEffect[] GenerateEffects() {
             
            //Assembly gets all available types from it that is classes and from the namespace
            var list = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.BaseType != null &&
            x.BaseType == typeof(Effect.AbstractAbilityEffect)).ToList();

            int amountOfEffects = UnityEngine.Random.Range(1, 3);
            List<Effect.AbstractAbilityEffect> effects = new();

            //Calculate energy from form and target
            float procentageReduction = 1 - (_Targeting.EnergyCost + _Form.EnergyCost);
            int energy = (int)(_AbilityEnergyCost * procentageReduction); //- Form and Targeting cost

            //Damage is mostly essential so there's a 70% chance to get it
            if (4 < UnityEngine.Random.Range(1, 11)) { 
                effects.Add(new Effect.EffectDamage(energy));
                list.Remove(typeof(Effect.EffectDamage));
            }
              
            for (int i = effects.Count; i < amountOfEffects; i++) {
                if (list.Count < 1) break;

                Effect.AbstractAbilityEffect newEffect = CreateRandomAllowedType<Effect.AbstractAbilityEffect>(list, energy);

                effects.Add(newEffect); 
                list.Remove(newEffect.GetType());

            } 
             
            return effects.ToArray();
        }

        private AbstractType CreateRandomAllowedType<AbstractType>(List<Type> typeList, int energy) where AbstractType : class{

            var filterList = typeList.Where(x => RequirementCheck(x)).ToList();

            int listIndex = UnityEngine.Random.Range(0, filterList.Count);

            //Had ability input before. It was changed but might be changed back
            return (AbstractType)Activator.CreateInstance(filterList[listIndex], energy);
        }

        private bool RequirementCheck(Type type) {
             
            Attribute[] attributes = type.GetCustomAttributes(typeof(AbstractAttributeAbilityRequirements)).ToArray();


            bool requirementsMeet = (attributes.Where(x => ((AbstractAttributeAbilityRequirements)x).CheckRequirements(this)).ToArray() !=
                Array.Empty<Attribute>());

            return (attributes.Count() == 0) || requirementsMeet;
        }


        private Perk.AbstractAbilityPerk GetRandomPerk() {

            var list = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.BaseType != null &&
            x.BaseType == typeof(Perk.AbstractAbilityPerk)).ToList();

            //Remove duplicates
            _AbilityPerks.ForEach(perk => list.Remove(perk.GetType())); 
              
            if (list.Count < 1) return null;

            Perk.AbstractAbilityPerk newPerk = CreateRandomAllowedType<Perk.AbstractAbilityPerk>(list, _AbilityEnergyCost);

            //Add perk
            return newPerk; 
             
        }

        private SubAbility GetRandomSubAbility(int energy) {

            //Get Random subAbility
            int _SubAbilityCreationCost = (int)((energy - 100) * 0.75f);

            return new(_SubAbilityCreationCost, _AbilityElement);
        }

        #endregion

    }
}
