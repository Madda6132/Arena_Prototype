namespace RPG.Statistics {
    
    public interface IStatisticsContainer {

        public void CreateStat<TStat>(TStat stat) where TStat : IStatistic;
        public void UpdateStat<TStat, TChange>(TChange changeValue) where TStat : abstractStatistic<TChange>;
         
        public void DeleteStat<TStat>(TStat stat) where TStat : IStatistic;
        public TStat ReadStat<TStat>() where TStat : IStatistic;
    }

}

