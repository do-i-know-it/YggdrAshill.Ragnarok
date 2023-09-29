namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to create <see cref="IObjectScope"/>.
    /// </summary>
    public interface IObjectContext : IObjectContainer
    {
        /// <summary>
        /// Creates <see cref="IObjectScope"/>.
        /// </summary>
        /// <returns>
        /// <see cref="IObjectScope"/> created.
        /// </returns>
        IObjectScope CreateScope();
    }
}
