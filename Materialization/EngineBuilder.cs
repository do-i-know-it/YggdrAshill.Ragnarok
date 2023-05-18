using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Motorization;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Materialization
{
    /// <summary>
    /// Implementation of <see cref="IEngineBuilder"/> with <see cref="IRegistryBuilder"/>.
    /// </summary>
    public sealed class EngineBuilder :
        IEngineBuilder
    {
        private readonly IRegistryBuilder registryBuilder;

        /// <summary>
        /// Constructor of <see cref="EngineBuilder"/>.
        /// </summary>
        /// <param name="registryBuilder"></param>
        public EngineBuilder(IRegistryBuilder registryBuilder)
        {
            this.registryBuilder = registryBuilder;
        }

        public IInstantiation GetInstantiation(Type type, IReadOnlyList<IParameter> parameterList)
        {
            // TODO: cache function.
            var activation = TypeAnalyzer.GetActivation(type, CreateActivation);

            return new ActivateToInstantiate(activation, parameterList);
        }
        private IActivation CreateActivation(Type type)
        {
            return registryBuilder.CreateActivation(type);
        }

        public IInjection GetFieldInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            // TODO: cache function.
            var infusion = TypeAnalyzer.GetFieldInjection(type, CreateFieldInfusion);

            return new InfuseToInject(infusion, parameterList);
        }
        private IInfusion CreateFieldInfusion(Type type)
        {
            return registryBuilder.CreateFieldInfusion(type);
        }

        public IInjection GetPropertyInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            // TODO: cache function.
            var infusion = TypeAnalyzer.GetPropertyInjection(type, CreatePropertyInfusion);

            return new InfuseToInject(infusion, parameterList);
        }
        private IInfusion CreatePropertyInfusion(Type type)
        {
            return registryBuilder.CreatePropertyInfusion(type);
        }

        public IInjection GetMethodInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            // TODO: cache function.
            var infusion = TypeAnalyzer.GetMethodInjection(type, CreateMethodInfusion);

            return new InfuseToInject(infusion, parameterList);
        }
        private IInfusion CreateMethodInfusion(Type type)
        {
            return registryBuilder.CreateMethodInfusion(type);
        }

        /// <inheritdoc/>
        public IEngine Build(IEnumerable<IDescription> descriptionList)
        {
            var registry = registryBuilder.Build(descriptionList, out var registrationList);

            TypeAnalyzer.Validate(registrationList, registry);

            var engine = new Engine(registry);

            return engine;
        }
    }
}
