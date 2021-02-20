namespace YggdrAshill.Ragnarok.Progression
{
    /// <summary>
    /// Token to activate.
    /// </summary>
    public interface IActivation
    {
        /// <summary>
        /// Activates.
        /// </summary>
        /// <returns>
        /// <see cref="IExecution"/> to execute.
        /// </returns>
        IExecution Activate();
    }
}
