namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines parameter to inject a dependency into.
    /// </summary>
    public interface IParameter
    {
        /// <summary>
        /// Gets <paramref name="instance"/> if this is for <paramref name="argument"/>.
        /// </summary>
        /// <param name="argument">
        /// <see cref="Argument"/> to resolve.
        /// </param>
        /// <param name="instance">
        /// <see cref="object"/> to resolve.
        /// </param>
        /// <returns>
        /// True if this is for <paramref name="argument"/>.
        /// </returns>
        bool CanResolve(Argument argument, out object instance);
    }
}
