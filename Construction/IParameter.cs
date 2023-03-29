using System;

namespace YggdrAshill.Ragnarok.Construction
{
    /// <summary>
    /// Defines parameter to inject from external.
    /// </summary>
    public interface IParameter
    {
        /// <summary>
        /// <see cref="Type"/> of parameter.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Parameter name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Parameter value.
        /// </summary>
        object Instance { get; }
    }
}
