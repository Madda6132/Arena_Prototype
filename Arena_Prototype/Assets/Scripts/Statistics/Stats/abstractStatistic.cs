using System.Collections.Generic;
using UnityEngine;

namespace RPG.Statistics { 
    /// <summary>
    /// Keeps track of all TValue that adds to this statistic and sends out information about changes or calls
    /// </summary>
    /// <typeparam name="TValue">Value the Statistic should keep track of</typeparam>
    public abstract class abstractStatistic<TValue> : IStatistic {

        [SerializeField] protected TValue baseValue;
        protected List<IResultUpdateListener<TValue>> UpdateListener {
            get {
                if (updateListener == null) updateListener = new();

                return updateListener;
            }
            set => updateListener = value;
        }
        protected List<IStatisticsFlatModifier<TValue>> FlatBonuses { 
            get { 
                if(flatBonuses == null) flatBonuses = new();

                return flatBonuses;
            } 
            set => flatBonuses = value; }
        protected List<IStatisticsPercentModifier> ProcentBonuses {
            get {
                if (procentBonuses == null) procentBonuses = new();

                return procentBonuses;
            }
            set => procentBonuses = value;
        }


        private List<IResultUpdateListener<TValue>> updateListener;
        private List<IStatisticsPercentModifier> procentBonuses;
        private List<IStatisticsFlatModifier<TValue>> flatBonuses;

        public abstractStatistic(TValue basevalue = default) {

            this.baseValue = basevalue;
        }
       
        public TValue GetStatvalue(IStatisticsController statisticsController) => Formula(statisticsController);
        public void UpdateAllListener(TValue forumlaValue) {
             
            UpdateListener.ForEach( x => x.OnResultUpdate(forumlaValue));
        }
        public StatInfo<TValue> GetValueInfo => new StatInfo<TValue>(baseValue, GetFlatBonuses(), GetProcentBonuses() * 100);
        public void SetValue(TValue baseValue) { this.baseValue = baseValue; }

        public void AddFlatModifier(IStatisticsFlatModifier<TValue> modifier) {

            if (!FlatBonuses.Contains(modifier)) FlatBonuses.Add(modifier);
        }
        public void RemoveFlatModifier(IStatisticsFlatModifier<TValue> modifier) {

            if (FlatBonuses.Contains(modifier)) FlatBonuses.Remove(modifier);
        }
        public void AddProcentModifier(IStatisticsPercentModifier modifier) {

            if (!ProcentBonuses.Contains(modifier)) ProcentBonuses.Add(modifier);
        }
        public void RemoveProcentModifier(IStatisticsPercentModifier modifier) {

            if (ProcentBonuses.Contains(modifier)) ProcentBonuses.Remove(modifier);
        }
        public void AddUpdateListener(IResultUpdateListener<TValue> listener) {

            if (!UpdateListener.Contains(listener)) UpdateListener.Add(listener);
        }
        public void RemoveUpdateListener(IResultUpdateListener<TValue> listener) {

            if (UpdateListener.Contains(listener)) UpdateListener.Add(listener);
        }
        protected float GetProcentBonuses() {
            float bonuses = 1;
            ProcentBonuses?.ForEach(x => bonuses *= x.GetPercentModifier());
            return bonuses;
        }

        public abstract void AddValue(TValue addingValue);

        abstract protected TValue GetFlatBonuses();

        /// <summary>
        /// Formula calculates the total value from modifiers and through Controller can get other stats to modify the total value
        /// </summary> 
        /// <returns></returns>
        abstract protected TValue Formula(IStatisticsController statisticsController);
         
    }

    public struct StatInfo<TReturn> {

        TReturn baseValue;
        TReturn flatBonus;
        float procentBonus;

        public StatInfo(TReturn baseValue, TReturn flatBonus, float procentBonus) {
            this.baseValue = baseValue;
            this.flatBonus = flatBonus;
            this.procentBonus = procentBonus;
        }

        public TReturn BaseValue => baseValue;
        public TReturn FlatBonus => flatBonus;
        public float ProcentBonus => procentBonus;

    }

}

    