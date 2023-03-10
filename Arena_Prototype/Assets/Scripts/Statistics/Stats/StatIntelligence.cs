using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Statistics {
    [System.Serializable]
    public class StatIntelligence : abstractStatistic<int> {

        string statisticName = "Intelligence";

        public StatIntelligence(int basevalue = 0) : base(basevalue) {
        }

        //Static cant be in another class so make sure to Copy the block "Static Statistic interface"
        #region Static Statistic interface


        public static int GetValue(IStatisticsController statisticsController) => statisticsController.ReadStatValue<StatIntelligence, int>();
        public static StatInfo<int> GetInfo(IStatisticsController statisticsController) => statisticsController.ReadStatInfo<StatIntelligence, int>();
        public static void AddListiner(IStatisticsController controller, IResultUpdateListener<int> listiner) => controller.AddListener<StatIntelligence, int>(listiner);
        public static void RemoveListiner(IStatisticsController controller, IResultUpdateListener<int> listiner) => controller.RemoveListener<StatIntelligence, int>(listiner);
        public static void AddFlatModifier(IStatisticsController controller, IStatisticsFlatModifier<int> modifier) => controller.AddFlatModifier<StatIntelligence, int>(modifier);
        public static void RemoveFlatModifier(IStatisticsController controller, IStatisticsFlatModifier<int> modifier) => controller.RemoveFlatModifier<StatIntelligence, int>(modifier);
        public static void AddProcentangeModifier(IStatisticsController controller, IStatisticsPercentModifier modifier) => controller.AddPercentangeModifier<StatIntelligence, int>(modifier);
        public static void RemoveProcentangeModifier(IStatisticsController controller, IStatisticsPercentModifier modifier) => controller.RemovePercentangeModifier<StatIntelligence, int>(modifier);
        public static void UpdateStat(IStatisticsController controller, int newValue) => controller.UpdateStat<StatIntelligence, int>(newValue);
        #endregion

        public string GetName => statisticName;

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
