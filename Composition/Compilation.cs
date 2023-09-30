using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation of <see cref="ICompilation"/> with <see cref="ISelector"/> and <see cref="ISolver"/>.
    /// </summary>
    public sealed class Compilation : ICompilation
    {
        private readonly ISelector selector;
        private readonly ISolver solver;

        private readonly Func<Type, IActivation> createActivation;
        private readonly Func<Type, IInfusion> createFieldInfusion;
        private readonly Func<Type, IInfusion> createPropertyInfusion;
        private readonly Func<Type, IInfusion> createMethodInfusion;

        /// <summary>
        /// Creates <see cref="Compilation"/>.
        /// </summary>
        /// <param name="selector">
        /// <see cref="ISelector"/> for <see cref="Compilation"/>.
        /// </param>
        /// <param name="solver">
        /// <see cref="ISolver"/> for <see cref="Compilation"/>.
        /// </param>
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

            if (ServiceBundleDescription.TryGetElementType(type, out var elementType))
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
