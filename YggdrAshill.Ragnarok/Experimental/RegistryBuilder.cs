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

        private readonly Func<Type, IActivation> activation;
        private readonly Func<Type, IInfusion> fieldInfusion;
        private readonly Func<Type, IInfusion> propertyInfusion;
        private readonly Func<Type, IInfusion> methodInfusion;

        public RegistryBuilder(ISelector selector, ISolver solver)
        {
            this.selector = selector;
            this.solver = solver;

            activation = CreateActivation;
            fieldInfusion = CreateFieldInfusion;
            propertyInfusion = CreatePropertyInfusion;
            methodInfusion = CreateMethodInfusion;
        }

        private readonly TypeAnalyzer typeAnalyzer = new TypeAnalyzer();

        public IActivation GetActivation(Type type)
        {
            return typeAnalyzer.GetActivation(type, activation);
        }
        private IActivation CreateActivation(Type type)
        {
            if (type.IsArray)
            {
                return solver.CreateCollectionActivation(type.GetElementType()!);
            }

            if (ServiceBundleRegistration.TryGetElementType(type, out var elementType))
            {
                return GetActivation(selector.GetServiceBundleType(elementType));
            }

            if (LocalInstanceListRegistration.TryGetReadOnlyListType(type, out elementType, out _))
            {
                return GetActivation(solver.GetLocalInstanceListType(elementType));
            }

            var injection = selector.CreateConstructorInjection(type);

            return solver.CreateActivation(injection);
        }

        public IInfusion GetFieldInfusion(Type type)
        {
            return typeAnalyzer.GetFieldInfusion(type, fieldInfusion);
        }
        private IInfusion CreateFieldInfusion(Type type)
        {
            var injection = selector.CreateFieldInjection(type);

            return solver.CreateFieldInfusion(injection);
        }

        public IInfusion GetPropertyInfusion(Type type)
        {
            return typeAnalyzer.GetPropertyInfusion(type, propertyInfusion);
        }
        private IInfusion CreatePropertyInfusion(Type type)
        {
            var injection = selector.CreatePropertyInjection(type);

            return solver.CreatePropertyInfusion(injection);
        }

        public IInfusion GetMethodInfusion(Type type)
        {
            return typeAnalyzer.GetMethodInfusion(type, methodInfusion);
        }
        private IInfusion CreateMethodInfusion(Type type)
        {
            var injection = selector.CreateMethodInjection(type);

            return solver.CreateMethodInfusion(injection);
        }

        public IRegistry Build(IEnumerable<IDescription> descriptionList)
        {
            using var factory = new RegistryFactory(this, descriptionList);

            var registry = factory.Create(out var registrationList);

            typeAnalyzer.Validate(registrationList, registry);

            return registry;
        }
    }
}
