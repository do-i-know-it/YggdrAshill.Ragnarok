namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to select <see cref="IStatement"/> for counting.
    /// </summary>
    public interface IStatementSelection
    {
        /// <summary>
        /// Detects <see cref="IStatement"/> to select.
        /// </summary>
        /// <param name="statement">
        /// <see cref="IStatement"/> to select.
        /// </param>
        /// <returns>
        /// True if <paramref name="statement"/> is satisfied with condition.
        /// </returns>
        bool IsSatisfied(IStatement statement);
    }
}
