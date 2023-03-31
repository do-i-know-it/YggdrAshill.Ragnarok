namespace YggdrAshill.Ragnarok.Construction
{
    /// <summary>
    /// Defines period of life for an instance resolved.
    /// </summary>
    public enum Lifetime
    {
        /// <summary>
        /// Instantiates per request.
        /// </summary>
        Temporal,
        /// <summary>
        /// Instantiates per scope.
        /// </summary>
        Local,
        /// <summary>
        /// Instantiates per service.
        /// </summary>
        Global,
    }
}
