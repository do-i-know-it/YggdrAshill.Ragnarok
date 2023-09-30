using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation <see cref="ISolver"/> with Reflection.
    /// </summary>
    public sealed class ReflectionSolver : ISolver
    {
        /// <summary>
        /// Singleton instance of <see cref="ReflectionSolver"/>.
        /// </summary>
        public static ReflectionSolver Instance { get; } = new();

        private ReflectionSolver()
        {

        }

        /// <inheritdoc/>
        public IActivation CreateActivation(ConstructorInjection injection)
        {
            return new ReflectionActivation(injection);
        }

        /// <inheritdoc/>
        public IInfusion CreateFieldInfusion(FieldInjection injection)
        {
            return new ReflectionFieldInfusion(injection);
        }

        /// <inheritdoc/>
        public IInfusion CreatePropertyInfusion(PropertyInjection injection)
        {
            return new ReflectionPropertyInfusion(injection);
        }

        /// <inheritdoc/>
        public IInfusion CreateMethodInfusion(MethodInjection injection)
        {
            return new ReflectionMethodInfusion(injection);
        }

        /// <inheritdoc/>
        public IActivation CreateCollectionActivation(Type elementType)
        {
            return new CollectionActivation(elementType);
        }
    }
}
