namespace RPG.Statistics {
    public interface IStatisticsController {

        public void CreateStat<TStat>(TStat stat) where TStat : IStatistic;
        public void UpdateStat<TStat, TChange>(TChange changeValue) where TStat : abstractStatistic<TChange>;

        public void AddFlatModifier<TStat, TModifier>(IStatisticsFlatModifier<TModifier> modifier) where TStat : IStatistic;
        public void RemoveFlatModifier<TStat, TModifier>(IStatisticsFlatModifier<TModifier> modifier) where TStat : IStatistic;

        public void AddPercentangeModifier<TStat, TChangeValue>(IStatisticsPercentModifier modifier) where TStat : IStatistic;
        public void RemovePercentangeModifier<TStat, TChangeValue>(IStatisticsPercentModifier modifier) where TStat : IStatistic;

        public void AddListener<TStat, TModifier>(IResultUpdateListener<TModifier> listener) where TStat : IStatistic;
        public void RemoveListener<TStat, TModifier>(IResultUpdateListener<TModifier> listener) where TStat : IStatistic;

        public TReturn ReadStatValue<TStat, TReturn>() where TStat : IStatistic; 
        public StatInfo<TReturn> ReadStatInfo<TStat, TReturn>() where TStat : IStatistic;
    }

}
