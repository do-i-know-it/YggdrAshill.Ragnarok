namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to instantiate.
    /// </summary>
    public interface IInstantiation
    {
        /// <summary>
        /// Creates an instance with <paramref name="resolver"/>.
        /// </summary>
        /// <param name="resolver">
        /// <see cref="IResolver"/> to instantiate.
        /// </param>
        /// <returns>
        /// <see cref="object"/> instantiated.
        /// </returns>
        object Instantiate(IResolver resolver);
    }
}
