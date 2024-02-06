namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to instantiate or inject into instance.
    /// </summary>
    public interface ICompilation
    {
        /// <summary>
        /// Creates <see cref="IInstantiation"/> for <paramref name="request"/> to instantiate.
        /// </summary>
        /// <param name="request">
        /// <see cref="DependencyInjectionRequest"/> to create.
        /// </param>
        /// <returns>
        /// <see cref="IInstantiation"/> created.
        /// </returns>
        IInstantiation CreateInstantiation(DependencyInjectionRequest request);

        /// <summary>
        /// Creates <see cref="IInjection"/> for <paramref name="request"/> to inject dependencies into fields.
        /// </summary>
        /// <param name="request">
        /// <see cref="DependencyInjectionRequest"/> to create.
        /// </param>
        /// <returns>
        /// <see cref="IInjection"/> created.
        /// </returns>
        IInjection CreateFieldInjection(DependencyInjectionRequest request);

        /// <summary>
        /// Creates <see cref="IInjection"/> for <paramref name="request"/> to inject dependencies into properties.
        /// </summary>
        /// <param name="request">
        /// <see cref="DependencyInjectionRequest"/> to create.
        /// </param>
        /// <returns>
        /// <see cref="IInjection"/> created.
        /// </returns>
        IInjection CreatePropertyInjection(DependencyInjectionRequest request);

        /// <summary>
        /// Creates <see cref="IInjection"/> for <paramref name="request"/> to inject dependencies into method.
        /// </summary>
        /// <param name="request">
        /// <see cref="DependencyInjectionRequest"/> to create.
        /// </param>
        /// <returns>
        /// <see cref="IInjection"/> created.
        /// </returns>
        IInjection CreateMethodInjection(DependencyInjectionRequest request);
    }
}
