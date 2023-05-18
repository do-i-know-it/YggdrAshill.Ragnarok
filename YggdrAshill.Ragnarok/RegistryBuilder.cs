using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Materialization;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation of <see cref="IRegistryBuilder"/> using <see cref="ISelector"/> and <see cref="ISolver"/>.
    /// </summary>
    public sealed class RegistryBuilder :
        IRegistryBuilder
    {
        private readonly ISelector selector;
        private readonly ISolver solver;

        public RegistryBuilder(ISelector selector, ISolver solver)
        {
            this.selector = selector;
            this.solver = solver;
        }

        private readonly TypeAnalyzer typeAnalyzer = new TypeAnalyzer();

        public IActivation GetActivation(Type type)
        {
            // TODO: cache function.
            return typeAnalyzer.GetActivation(type, CreateActivation);
        }
        private IActivation CreateActivation(Type type)
        {
            if (type.IsArray)
            {
                return solver.CreateCollectionActivation(type.GetElementType()!);
            }

            if (LocalInstanceListRegistration.TryGetReadOnlyListType(type, out var elementType, out _))
            {
                return GetActivation(solver.GetLocalInstanceListType(elementType));
            }

            var injection = selector.CreateConstructorInjection(type);

            return solver.CreateActivation(injection);
        }

        public IInfusion GetFieldInfusion(Type type)
        {
            // TODO: cache function.
            return typeAnalyzer.GetFieldInfusion(type, CreateFieldInfusion);
        }
        private IInfusion CreateFieldInfusion(Type type)
        {
            var injection = selector.CreateFieldInjection(type);

            return solver.CreateFieldInfusion(injection);
        }

        public IInfusion GetPropertyInfusion(Type type)
        {
            // TODO: cache function.
            return typeAnalyzer.GetPropertyInfusion(type, CreatePropertyInfusion);
        }
        private IInfusion CreatePropertyInfusion(Type type)
        {
            var injection = selector.CreatePropertyInjection(type);

            return solver.CreatePropertyInfusion(injection);
        }

        public IInfusion GetMethodInfusion(Type type)
        {
            // TODO: cache function.
            return typeAnalyzer.GetMethodInfusion(type, CreateMethodInfusion);
        }
        private IInfusion CreateMethodInfusion(Type type)
        {
            var injection = selector.CreateMethodInjection(type);

            return solver.CreateMethodInfusion(injection);
        }

        public IRegistry Build(IEnumerable<IDescription> descriptionList)
        {
            using (var converter = new ConvertDescriptionListToEngine(this, descriptionList))
            {
                var registry = converter.Convert(out var registrationList);

                typeAnalyzer.Validate(registrationList, registry);

                return registry;
            }
        }
    }
}
