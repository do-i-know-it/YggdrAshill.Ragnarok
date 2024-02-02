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
        public IActivation CreateActivation(ConstructorInjectionRequest request)
        {
            return new ReflectionActivation(request);
        }

        /// <inheritdoc/>
        public IInfusion CreateFieldInfusion(FieldInjectionRequest request)
        {
            return new ReflectionFieldInfusion(request);
        }

        /// <inheritdoc/>
        public IInfusion CreatePropertyInfusion(PropertyInjectionRequest request)
        {
            return new ReflectionPropertyInfusion(request);
        }

        /// <inheritdoc/>
        public IInfusion CreateMethodInfusion(MethodInjectionRequest request)
        {
            return new ReflectionMethodInfusion(request);
        }

        /// <inheritdoc/>
        public IActivation CreateCollectionActivation(Type elementType)
        {
            return new CollectionActivation(elementType);
        }
    }
}
