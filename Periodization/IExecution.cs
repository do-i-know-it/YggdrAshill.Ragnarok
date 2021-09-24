namespace YggdrAshill.Ragnarok.Periodization
{
    /// <summary>
    /// Token to run something.
    /// </summary>
    public interface IExecution
    {
        /// <summary>
        /// Runs. 
        /// </summary>
        void Execute();
    }
}
