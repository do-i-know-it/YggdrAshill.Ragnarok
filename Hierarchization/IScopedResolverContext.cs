namespace YggdrAshill.Ragnarok.Hierarchization
{
    /// <summary>
    /// Defines how to build <see cref="IScopedResolver"/>.
    /// </summary>
    public interface IScopedResolverContext :
        IScopedResolverContainer
    {
        /// <summary>
        /// Creates a new <see cref="IScopedResolver"/>.
        /// </summary>
        /// <returns>
        /// <see cref="IScopedResolver"/> created.
        /// </returns>
        IScopedResolver Build();
    }
}
