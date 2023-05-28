namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to register dependencies to <see cref="IScopedResolver"/>.
    /// </summary>
    public interface IScopedResolverContainer :
        ICompilation
    {
        /// <summary>
        /// Adds a dependency.
        /// </summary>
        /// <param name="composition">
        /// <see cref="IComposition"/> to define dependencies.
        /// </param>
        void Register(IComposition composition);
    }
}
