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
            var activation = registryBuilder.GetActivation(type);

            return new ActivateToInstantiate(activation, parameterList);
        }

        public IInjection GetFieldInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            var infusion = registryBuilder.GetFieldInfusion(type);

            return new InfuseToInject(infusion, parameterList);
        }

        public IInjection GetPropertyInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            var infusion = registryBuilder.GetPropertyInfusion(type);

            return new InfuseToInject(infusion, parameterList);
        }

        public IInjection GetMethodInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            var infusion = registryBuilder.GetMethodInfusion(type);

            return new InfuseToInject(infusion, parameterList);
        }

        /// <inheritdoc/>
        public IEngine Build(IEnumerable<IDescription> descriptionList)
        {
            var registry = registryBuilder.Build(descriptionList, out var registrationList);

            var engine = new Engine(registry);

            return engine;
        }
    }
}
