namespace YggdrAshill.Ragnarok.Construction
{
    /// <summary>
    /// Defines how to inject objects to resolve.
    /// </summary>
    public interface IInjection
    {
        /// <summary>
        /// Add dependencies into <paramref name="instance"/> with <paramref name="resolver"/>.
        /// </summary>
        /// <param name="resolver">
        /// <see cref="IResolver"/> to inject.
        /// </param>
        /// <param name="instance">
        /// <see cref="object"/> injected.
        /// </param>
        void Inject(IResolver resolver, object instance);
    }
}
