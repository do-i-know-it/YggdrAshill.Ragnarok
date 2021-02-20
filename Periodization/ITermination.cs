namespace YggdrAshill.Ragnarok.Periodization
{
    /// <summary>
    /// Token to stop, end or finalize.
    /// </summary>
    public interface ITermination
    {
        /// <summary>
        /// Terminates. 
        /// </summary>
        void Terminate();
    }
}
