namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to count <see cref="IStatement"/>s satisfied with condition.
    /// </summary>
    public interface IStatementCondition
    {
        /// <summary>
        /// Detects <see cref="IStatement"/> to select.
        /// </summary>
        /// <param name="statement">
        /// <see cref="IStatement"/> to select.
        /// </param>
        /// <returns>
        /// True if <paramref name="statement"/> is satisfied with this.
        /// </returns>
        bool IsSatisfied(IStatement statement);
    }
}
