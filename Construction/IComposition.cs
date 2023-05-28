namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to define <see cref="IDescription"/>.
    /// </summary>
    public interface IComposition
    {
        /// <summary>
        /// Creates an <see cref="IDescription"/>.
        /// </summary>
        /// <returns>
        /// <see cref="IDescription"/> created.
        /// </returns>
        IDescription Compose();
    }
}
