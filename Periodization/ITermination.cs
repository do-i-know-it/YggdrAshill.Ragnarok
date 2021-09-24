namespace YggdrAshill.Ragnarok.Periodization
{
    /// <summary>
    /// Token to finalize something.
    /// </summary>
    public interface ITermination
    {
        /// <summary>
        /// Finalizes. 
        /// </summary>
        void Terminate();
    }
}
