using System;

namespace YggdrAshill.Ragnarok
{
    public sealed class Compilation : ICompilationV2
    {
        private readonly ISelector selector;
        private readonly ISolver solver;

        private readonly Func<Type, IActivation> createActivation;
        private readonly Func<Type, IInfusion> createFieldInfusion;
        private readonly Func<Type, IInfusion> createPropertyInfusion;
        private readonly Func<Type, IInfusion> createMethodInfusion;

        public Compilation(ISelector selector, ISolver solver)
        {
            this.selector = selector;
            this.solver = solver;

            createActivation = CreateActivation;
            createFieldInfusion = CreateFieldInfusion;
            createPropertyInfusion = CreatePropertyInfusion;
            createMethodInfusion = CreateMethodInfusion;
        }

        /// <inheritdoc/>
        public IActivation GetActivation(Type type)
        {
            return TypeAnalysis.GetActivation(type, createActivation);
        }
        private IActivation CreateActivation(Type type)
        {
            if (type.IsArray)
            {
                return solver.CreateCollectionActivation(type.GetElementType()!);
            }

            if (ServiceBundleDepiction.TryGetElementType(type, out var elementType))
            {
                return GetActivation(selector.GetServiceBundleType(elementType));
            }

            var injection = selector.CreateConstructorInjection(type);

            return solver.CreateActivation(injection);
        }

        /// <inheritdoc/>
        public IInfusion GetFieldInfusion(Type type)
        {
            return TypeAnalysis.GetFieldInfusion(type, createFieldInfusion);
        }
        private IInfusion CreateFieldInfusion(Type type)
        {
            var injection = selector.CreateFieldInjection(type);

            return solver.CreateFieldInfusion(injection);
        }

        /// <inheritdoc/>
        public IInfusion GetPropertyInfusion(Type type)
        {
            return TypeAnalysis.GetPropertyInfusion(type, createPropertyInfusion);
        }
        private IInfusion CreatePropertyInfusion(Type type)
        {
            var injection = selector.CreatePropertyInjection(type);

            return solver.CreatePropertyInfusion(injection);
        }

        /// <inheritdoc/>
        public IInfusion GetMethodInfusion(Type type)
        {
            return TypeAnalysis.GetMethodInfusion(type, createMethodInfusion);
        }
        private IInfusion CreateMethodInfusion(Type type)
        {
            var injection = selector.CreateMethodInjection(type);

            return solver.CreateMethodInfusion(injection);
        }
    }
}
