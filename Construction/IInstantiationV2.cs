namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to instantiate instance resolved dependency.
    /// </summary>
    public interface IInstantiationV2
    {
        /// <summary>
        /// Creates instance with <paramref name="resolver"/>.
        /// </summary>
        /// <param name="resolver">
        /// <see cref="IObjectResolver"/> to instantiate.
        /// </param>
        /// <returns>
        /// <see cref="object"/> instantiated.
        /// </returns>
        object Instantiate(IObjectResolver resolver);
    }
}
