namespace YggdrAshill.Ragnarok.Progression
{
    /// <summary>
    /// Detects condition is satisfied.
    /// </summary>
    public interface ICondition
    {
        /// <summary>
        /// This is satisfied.
        /// </summary>
        bool IsSatisfied { get; }
    }
}
