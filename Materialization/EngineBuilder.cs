using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Motorization;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Materialization
{
    /// <summary>
    /// Implementation of <see cref="IEngineBuilder"/> using <see cref="IRegistryBuilder"/>.
    /// </summary>
    public sealed class EngineBuilder :
        IEngineBuilder
    {
        private readonly IRegistryBuilder registryBuilder;

        /// <summary>
        /// Constructor of <see cref="EngineBuilder"/>.
        /// </summary>
        /// <param name="registryBuilder">
        /// <see cref="IRegistryBuilder"/> to instantiate <see cref="EngineBuilder"/>.
        /// </param>
        public EngineBuilder(IRegistryBuilder registryBuilder)
        {
            this.registryBuilder = registryBuilder;
        }

        /// <summary>
        /// Gets <see cref="IActivation"/> of <see cref="Type"/>, then creates <see cref="IInstantiation"/>.
        /// </summary>
        /// <param name="type">
        /// <see cref="Type"/> to instantiate.
        /// </param>
        /// <param name="parameterList">
        /// <see cref="IParameter"/> to instantiate.
        /// </param>
        /// <returns>
        /// <see cref="IInstantiation"/> obtained.
        /// </returns>
        public IInstantiation GetInstantiation(Type type, IReadOnlyList<IParameter> parameterList)
        {
            var activation = registryBuilder.GetActivation(type);

            return new ActivateToInstantiate(activation, parameterList);
        }

        /// <summary>
        /// Gets <see cref="IInfusion"/> for fields of <see cref="Type"/>, then creates <see cref="IInjection"/>.
        /// </summary>
        /// <param name="type">
        /// <see cref="Type"/> to inject.
        /// </param>
        /// <param name="parameterList">
        /// <see cref="IParameter"/> to inject.
        /// </param>
        /// <returns>
        /// <see cref="IInjection"/> obtained.
        /// </returns>
        public IInjection GetFieldInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            var infusion = registryBuilder.GetFieldInfusion(type);

            return new InfuseToInject(infusion, parameterList);
        }

        /// <summary>
        /// Gets <see cref="IInfusion"/> for properties of <see cref="Type"/>, then creates <see cref="IInjection"/>.
        /// </summary>
        /// <param name="type">
        /// <see cref="Type"/> to inject.
        /// </param>
        /// <param name="parameterList">
        /// <see cref="IParameter"/> to inject.
        /// </param>
        /// <returns>
        /// <see cref="IInjection"/> obtained.
        /// </returns>
        public IInjection GetPropertyInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            var infusion = registryBuilder.GetPropertyInfusion(type);

            return new InfuseToInject(infusion, parameterList);
        }

        /// <summary>
        /// Gets <see cref="IInfusion"/> for method of <see cref="Type"/>, then creates <see cref="IInjection"/>.
        /// </summary>
        /// <param name="type">
        /// <see cref="Type"/> to inject.
        /// </param>
        /// <param name="parameterList">
        /// <see cref="IParameter"/> to inject.
        /// </param>
        /// <returns>
        /// <see cref="IInjection"/> obtained.
        /// </returns>
        public IInjection GetMethodInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            var infusion = registryBuilder.GetMethodInfusion(type);

            return new InfuseToInject(infusion, parameterList);
        }

        /// <summary>
        /// Creates <see cref="IRegistry"/> from <see cref="IDescription"/>s with <see cref="IRegistryBuilder"/>,
        /// then creates <see cref="IEngine"/>.
        /// </summary>
        /// <param name="descriptionList">
        /// <see cref="IDescription"/> to build.
        /// </param>
        /// <returns>
        /// <see cref="IEngine"/> created.
        /// </returns>
        public IEngine Build(IEnumerable<IDescription> descriptionList)
        {
            var registry = registryBuilder.Build(descriptionList);

            var engine = new Engine(registry);

            return engine;
        }
    }
}
