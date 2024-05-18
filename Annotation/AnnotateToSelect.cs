using System.Reflection;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation of <see cref="IDependencySelection"/> with annotation.
    /// </summary>
    public sealed class AnnotateToSelect : IDependencySelection
    {
        /// <summary>
        /// Singleton of <see cref="AnnotateToSelect"/>.
        /// </summary>
        public static AnnotateToSelect Instance { get; } = new();

        private AnnotateToSelect()
        {
        }

        /// <inheritdoc/>
        public bool IsValid(ConstructorInfo info)
        {
            return info.IsDefined(typeof(InjectAttribute), true);
        }

        /// <inheritdoc/>
        public bool IsValid(FieldInfo info)
        {
            return info.IsDefined(typeof(InjectFieldAttribute), true);
        }

        /// <inheritdoc/>
        public bool IsValid(PropertyInfo info)
        {
            return info.IsDefined(typeof(InjectPropertyAttribute), true);
        }

        /// <inheritdoc/>
        public bool IsValid(MethodInfo info)
        {
            return info.IsDefined(typeof(InjectMethodAttribute), true);
        }
    }
}
