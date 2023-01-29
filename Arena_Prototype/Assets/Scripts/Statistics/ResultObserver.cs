using System;

namespace RPG.Statistics {
    /// <summary>
    /// The observer will listen to value change in AbstractStatistic and send the updated value to an action
    /// </summary>
    public class ResultObserver<AbstractStatistic, UpdateValue> : IResultUpdateListener<UpdateValue> where 
        AbstractStatistic : abstractStatistic<UpdateValue> {

        IStatisticsController statisticsController;
        Action<UpdateValue> updateMethod;

        /// The observer will listen to value change in AbstractStatistic and send the updated value to an action
        public ResultObserver(IStatisticsController statisticsController, Action<UpdateValue> updateMethod = null) {
            this.statisticsController = statisticsController;
            this.statisticsController.AddListener<AbstractStatistic, UpdateValue>(this);

            if(updateMethod != null) this.updateMethod += updateMethod;


        }
        public ResultObserver(Action<UpdateValue> updateMethod) {
            this.updateMethod += updateMethod;

        }

        public void AddUpdateMethod(Action<UpdateValue> updateMethod) => this.updateMethod += updateMethod;
        public void RemoveUpdateMethod(Action<UpdateValue> updateMethod) => this.updateMethod -= updateMethod;

        public void OnResultUpdate(UpdateValue result) => updateMethod?.Invoke(result);

        /*---Private---*/

        ~ResultObserver() {
            statisticsController?.RemoveListener<AbstractStatistic, UpdateValue>(this);

        }
    }
}

