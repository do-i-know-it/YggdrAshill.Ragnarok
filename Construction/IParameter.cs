using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines parameter to inject a dependency into.
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
