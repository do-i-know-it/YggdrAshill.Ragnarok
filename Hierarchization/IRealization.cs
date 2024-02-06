namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to realize instances to resolve dependencies.
    /// </summary>
    public interface IRealization
    {
        // TODO: object pooling.
        /// <summary>
        /// Realizes dependencies with <paramref name="resolver"/>.
        /// </summary>
        /// <param name="resolver">
        /// <see cref="IObjectResolver"/> to realize.
        /// </param>
        /// <returns>
        /// <see cref="object"/>s realized.
        /// </returns>
        object[] Realize(IObjectResolver resolver);
    }
}
