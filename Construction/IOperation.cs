namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to operate <see cref="IObjectResolver"/>.
    /// </summary>
    public interface IOperation
    {
        /// <summary>
        /// Operates <paramref name="resolver"/>.
        /// </summary>
        /// <param name="resolver">
        /// <see cref="IObjectResolver"/> created.
        /// </param>
        void Operate(IObjectResolver resolver);
    }
}
