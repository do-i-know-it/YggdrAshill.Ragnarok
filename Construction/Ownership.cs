namespace YggdrAshill.Ragnarok.Construction
{
    /// <summary>
    /// Defines permission to control an instance.
    /// </summary>
    public enum Ownership
    {
        /// <summary>
        /// Manage as an instance resolved internally.
        /// </summary>
        Internal,

        /// <summary>
        /// Leave as an instance resolved externally.
        /// </summary>
        External,
    }
}
