namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to build <see cref="IScope"/>.
    /// </summary>
    public interface IContext :
        IContainer
    {
        /// <summary>
        /// Creates a new <see cref="IScope"/>.
        /// </summary>
        /// <returns>
        /// <see cref="IScope"/> created.
        /// </returns>
        IScope Build();
    }
}
