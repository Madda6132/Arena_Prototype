namespace RPG.Statistics {
    /// <summary>
    /// Controls the interface logic when calling method related to statistic containers
    /// </summary>
    public class StatisticLogic : IStatisticsController {

        IStatisticsContainer container;

        public StatisticLogic() { 
            
            container = new StatisticsContainer();
        }

        public void AddFlatModifier<TStat, TModifier>(IStatisticsFlatModifier<TModifier> modifier) where TStat : IStatistic {
            IStatistic stat = container.ReadStat<TStat>();
            ((abstractStatistic<TModifier>)stat).AddFlatModifier(modifier);
            UpdateStatListeners<TStat, TModifier>(ReadStatValue<TStat, TModifier>());
        } 
        public void RemoveFlatModifier<TStat, TModifier>(IStatisticsFlatModifier<TModifier> modifier) where TStat : IStatistic {
            IStatistic stat = container.ReadStat<TStat>();
            ((abstractStatistic<TModifier>)stat).RemoveFlatModifier(modifier);
            UpdateStatListeners<TStat, TModifier>(ReadStatValue<TStat, TModifier>());
        } 
        public void AddPercentangeModifier<TStat, TChangeValue>(IStatisticsPercentModifier modifier) where TStat : IStatistic {
             IStatistic stat = container.ReadStat<TStat>();
            ((abstractStatistic<TChangeValue>)stat).AddProcentModifier(modifier);
            UpdateStatListeners<TStat, TChangeValue>(ReadStatValue<TStat, TChangeValue>());
        }
        public void RemovePercentangeModifier<TStat, TChangeValue>(IStatisticsPercentModifier modifier) where TStat : IStatistic {
            IStatistic stat = container.ReadStat<TStat>();
            ((abstractStatistic<TChangeValue>)stat).RemoveProcentModifier(modifier);
            UpdateStatListeners<TStat, TChangeValue>(ReadStatValue<TStat, TChangeValue>());
        } 
        public void AddListener<TStat, TModifier>(IResultUpdateListener<TModifier> listener) where TStat : IStatistic {
            IStatistic stat = container.ReadStat<TStat>();
            ((abstractStatistic<TModifier>)stat).AddUpdateListener(listener);
            UpdateStatListeners<TStat, TModifier>(ReadStatValue<TStat, TModifier>());
        }
        public void RemoveListener<TStat, TModifier>(IResultUpdateListener<TModifier> listener) where TStat : IStatistic {
            IStatistic stat = container.ReadStat<TStat>();
            ((abstractStatistic<TModifier>)stat).RemoveUpdateListener(listener);
            UpdateStatListeners<TStat, TModifier>(ReadStatValue<TStat, TModifier>());
        }

        public void CreateStat<TStat>(TStat stat) where TStat : IStatistic {

            container.CreateStat(stat);
        }
         

        public TStat ReadStatValue<TStat>() where TStat : IStatistic {
             
            return container.ReadStat<TStat>();
        }

        public StatInfo<TReturn> ReadStatInfo<TStat, TReturn>() where TStat : IStatistic {

            IStatistic stat = ReadStatValue<TStat>();
            return ((abstractStatistic<TReturn>)stat).GetValueInfo;
        }


        public void UpdateStat<TStat, TChange>(TChange changeValue) where TStat : abstractStatistic<TChange> {
            IStatistic stat = ReadStatValue<TStat>();
            ((abstractStatistic<TChange>)stat).SetValue(changeValue);
            UpdateStatListeners<TStat, TChange>(ReadStatValue<TStat, TChange>());
        }

        public TReturn ReadStatValue<TStat, TReturn>() where TStat : IStatistic {

            IStatistic stat = ReadStatValue<TStat>();
            return ((abstractStatistic<TReturn>)stat).GetStatvalue(this);
        }

        /*---Private---*/

        private void UpdateStatListeners<TStat, TUpdateValue>(TUpdateValue updateValue) where TStat : IStatistic {
            IStatistic stat = ReadStatValue<TStat>();
            ((abstractStatistic<TUpdateValue>)stat).UpdateAllListener(updateValue);
        }
         
    }

}

