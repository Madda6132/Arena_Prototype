using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Statistics {
    [System.Serializable]
    public class StatDexterity : abstractStatistic<int> {

        private static string statisticName = "Dexterity";
        


        public StatDexterity(int basevalue = 0) : base(basevalue) {
            
        }

        //Static cant be in another class so make sure to Copy the block "Static Statistic interface"
        #region Static Statistic interface


        public static int GetValue(IStatisticsController statisticsController) => statisticsController.ReadStatValue<StatDexterity, int>();
        public static StatInfo<int> GetInfo(IStatisticsController statisticsController) => statisticsController.ReadStatInfo<StatDexterity, int>();
        public static void AddListiner(IStatisticsController controller, IResultUpdateListener<int> listiner) => controller.AddListener<StatDexterity, int>(listiner);
        public static void RemoveListiner(IStatisticsController controller, IResultUpdateListener<int> listiner) => controller.RemoveListener<StatDexterity, int>(listiner);
        public static void AddFlatModifier(IStatisticsController controller, IStatisticsFlatModifier<int> modifier) => controller.AddFlatModifier<StatDexterity, int>(modifier);
        public static void RemoveFlatModifier(IStatisticsController controller, IStatisticsFlatModifier<int> modifier) => controller.RemoveFlatModifier<StatDexterity, int>(modifier);
        public static void AddProcentangeModifier(IStatisticsController controller, IStatisticsPercentModifier modifier) => controller.AddPercentangeModifier<StatDexterity, int>(modifier);
        public static void RemoveProcentangeModifier(IStatisticsController controller, IStatisticsPercentModifier modifier) => controller.RemovePercentangeModifier<StatDexterity, int>(modifier);
        public static void UpdateStat(IStatisticsController controller, int newValue) => controller.UpdateStat<StatDexterity, int>(newValue);
        #endregion
        
        public static string GetName => statisticName;

        public override void AddValue(int addingValue) {
            baseValue += addingValue;
        }

        /*---Protected---*/

        protected override int Formula(IStatisticsController statisticsController) {

            int totalValue = baseValue;
            totalValue += GetFlatBonuses();
            totalValue = (int)(totalValue * GetProcentBonuses());
            
            return totalValue;
        }

        protected override int GetFlatBonuses() {
            int bonuses = 0;
            FlatBonuses?.ForEach(x => bonuses += x.GetFlatModifier());
            return bonuses;
        }
    }
}
