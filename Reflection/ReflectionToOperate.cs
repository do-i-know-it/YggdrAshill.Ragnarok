using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation <see cref="IDependencyOperation"/> with Reflection.
    /// </summary>
    public sealed class ReflectionToOperate : IDependencyOperation
    {
        /// <summary>
        /// Singleton instance of <see cref="ReflectionToOperate"/>.
        /// </summary>
        public static ReflectionToOperate Instance { get; } = new();

        private ReflectionToOperate()
        {

        }

        /// <inheritdoc/>
        public IActivation CreateActivation(ConstructorInjectionRequest request)
        {
            return new ReflectionConstructorActivation(request);
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

        public IActivation CreateActivation(Type type)
        {
            return new ReflectionTypeActivation(type);
        }

        /// <inheritdoc/>
        public IActivation CreateCollectionActivation(Type elementType)
        {
            return new CollectionActivation(elementType);
        }
    }
}
