
    /// <summary>
    /// Listens to a statistic container value change
    /// </summary>
    /// <typeparam name="Result"></typeparam>
    public interface IResultUpdateListener<Result> {

        public void OnResultUpdate(Result result);
    }
