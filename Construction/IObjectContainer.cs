namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to resolve dependencies in <see cref="IObjectScope"/>.
    /// </summary>
    public interface IObjectContainer
    {
        /// <summary>
        /// <see cref="IObjectResolver"/> to resolve dependency.
        /// </summary>
        IObjectResolver Resolver { get; }

        /// <summary>
        /// <see cref="ICompilationV2"/> to resolve dependency.
        /// </summary>
        ICompilationV2 Compilation { get; }

        /// <summary>
        /// <see cref="IRegistrationV2"/> to resolve dependency.
        /// </summary>
        IRegistrationV2 Registration { get; }

        /// <summary>
        /// Creates <see cref="IObjectContext"/> to resolve dependency.
        /// </summary>
        /// <returns>
        /// <see cref="IObjectContext"/> created.
        /// </returns>
        IObjectContext CreateContext();
    }
}
