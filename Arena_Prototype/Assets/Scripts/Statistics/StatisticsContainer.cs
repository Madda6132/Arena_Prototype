using System.Collections.Generic;

namespace RPG.Statistics { 

    //Contains all the statistics a "Creature" has
    public class StatisticsContainer : IStatisticsContainer
    {
        Dictionary<System.Type, IStatistic> statCategory; 
        public StatisticsContainer(Dictionary<System.Type, IStatistic> statCategory = null) { 
            
            this.statCategory = statCategory ?? new Dictionary<System.Type, IStatistic>();
        }
          
        public void CreateStat<TStat>(TStat stat) where TStat : IStatistic {

            if (!statCategory.ContainsKey(typeof(TStat))) { 
                  
                statCategory.Add(typeof(TStat), stat);
            }
        }
         
        public void DeleteStat<T>(T stat) where T : IStatistic {
            throw new System.NotImplementedException();
        }

        public T ReadStat<T>() where T : IStatistic {

            return GetContainer<T>();
        }

        public void UpdateStat<TStat, TChange>(TChange baseValue) where TStat : abstractStatistic<TChange> { 

        IStatistic stat = GetContainer<TStat>();
            if (stat == null) throw new System.Exception("The IStatistic is null"); 

            ((abstractStatistic<TChange>)stat).SetValue(baseValue);
            
        }

        private T GetContainer<T>() where T : IStatistic {

            return (T)(statCategory[typeof(T)] ?? default);

        }
         
    }

}