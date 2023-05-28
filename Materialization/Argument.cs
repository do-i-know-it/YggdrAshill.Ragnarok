using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines argument to inject a dependency into.
    /// </summary>
    public sealed class Argument
    {
        /// <summary>
        /// Name of <see cref="Argument"/>.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// <see cref="Type"/> of <see cref="Argument"/>.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Constructor of <see cref="Argument"/>.
        /// </summary>
        /// <param name="name">
        /// <see cref="string"/> for <see cref="Name"/>.
        /// </param>
        /// <param name="type">
        /// <see cref="System.Type"/> for <see cref="Type"/>.
        /// </param>
        public Argument(string name, Type type)
        {
            Name = name;
            Type = type;
        }
    }
}
