namespace YggdrAshill.Ragnarok.Periodization
{
    /// <summary>
    /// Token to start, begin or initialize.
    /// </summary>
    public interface IOrigination
    {
        /// <summary>
        /// Originates.
        /// </summary>
        /// <returns>
        /// <see cref="ITermination"/> to terminate.
        /// </returns>
        ITermination Originate();
    }
}
