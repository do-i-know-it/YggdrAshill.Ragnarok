namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to resolve dependencies in <see cref="IObjectScope"/>.
    /// </summary>
    public interface IObjectContainer : IRegistration
    {
        /// <summary>
        /// <see cref="IObjectResolver"/> to resolve dependency.
        /// </summary>
        IObjectResolver Resolver { get; }

        /// <summary>
        /// <see cref="ICompilation"/> to resolve dependency.
        /// </summary>
        ICompilation Compilation { get; }

        /// <summary>
        /// Creates <see cref="IObjectContext"/> to resolve dependency.
        /// </summary>
        /// <returns>
        /// <see cref="IObjectContext"/> created.
        /// </returns>
        IObjectContext CreateContext();
    }
}
