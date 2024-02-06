namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines what to execute for <see cref="IObjectResolver"/>.
    /// </summary>
    public interface IInstruction
    {
        /// <summary>
        /// Executes for <paramref name="resolver"/>.
        /// </summary>
        /// <param name="resolver">
        /// <see cref="IObjectResolver"/> to execute.
        /// </param>
        void Execute(IObjectResolver resolver);
    }
}
