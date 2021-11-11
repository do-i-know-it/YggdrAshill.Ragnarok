namespace YggdrAshill.Ragnarok.Periodization
{
    /// <summary>
    /// Lifetime from <see cref="IOrigination"/> to <see cref="ITermination"/>.
    /// </summary>
    public interface ISpan
    {
        /// <summary>
        /// <see cref="IOrigination"/> to initialize.
        /// </summary>
        IOrigination Origination { get; }

        /// <summary>
        /// <see cref="ITermination"/> to finalize.
        /// </summary>
        ITermination Termination { get; }
    }
}
