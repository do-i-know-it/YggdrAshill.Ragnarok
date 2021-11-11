namespace YggdrAshill.Ragnarok.Periodization
{
    /// <summary>
    /// Lifecycle for <see cref="IExecution"/> between <see cref="IOrigination"/> and <see cref="ITermination"/> of <see cref="ISpan"/>.
    /// </summary>
    public interface ICycle
    {
        /// <summary>
        /// <see cref="ISpan"/> for <see cref="Execution"/>.
        /// </summary>
        ISpan Span { get; }

        /// <summary>
        /// <see cref="IExecution"/> to run.
        /// </summary>
        IExecution Execution { get; }
    }
}
