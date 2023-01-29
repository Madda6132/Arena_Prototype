namespace RPG.Statistics {
    /// <summary>
    /// The flat modifier adds the modifier to a statistic containers value
    /// </summary>
    /// <typeparam name="Modifier"></typeparam>
    public interface IStatisticsFlatModifier<Modifier> {
        public Modifier GetFlatModifier();
    }
}