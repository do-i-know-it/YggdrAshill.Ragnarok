namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to inject dependencies into instance to resolve.
    /// </summary>
    public interface IInjection
    {
        /// <summary>
        /// Injects dependencies into <paramref name="instance"/> with <paramref name="resolver"/>.
        /// </summary>
        /// <param name="resolver">
        /// <see cref="IObjectResolver"/> to inject.
        /// </param>
        /// <param name="instance">
        /// <see cref="object"/> to inject.
        /// </param>
        void Inject(IObjectResolver resolver, object instance);
    }
}
