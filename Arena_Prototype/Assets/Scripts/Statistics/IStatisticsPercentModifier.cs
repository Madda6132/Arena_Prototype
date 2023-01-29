namespace RPG.Statistics {
    /// <summary>
    /// Percent modifier adds together with outer modifiers and multiplies with the value.
    /// Value for ex, means 2 equal  +200%, 0.5f equals 50%
    /// </summary>
    public interface IStatisticsPercentModifier {
        public float GetPercentModifier();
    }
}