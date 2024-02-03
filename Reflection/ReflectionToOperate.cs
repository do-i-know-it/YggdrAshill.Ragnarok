namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation <see cref="IOperation"/> with Reflection.
    /// </summary>
    public sealed class ReflectionToOperate : IOperation
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
        public IActivation CreateCollectionActivation(CollectionInjectionRequest request)
        {
            return new CollectionActivation(request);
        }
    }
}
