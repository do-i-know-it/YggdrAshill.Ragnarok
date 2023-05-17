using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Motorization;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Materialization
{
    /// <summary>
    /// Implementation of <see cref="IEngineBuilder"/> with <see cref="ICodeBuilder"/>.
    /// </summary>
    public sealed class EngineBuilder :
        IEngineBuilder
    {
        private readonly ICodeBuilder codeBuilder;

        /// <summary>
        /// Constructor of <see cref="EngineBuilder"/>.
        /// </summary>
        /// <param name="codeBuilder"></param>
        public EngineBuilder(ICodeBuilder codeBuilder)
        {
            this.codeBuilder = codeBuilder;
        }

        public IInstantiation GetInstantiation(Type type, IReadOnlyList<IParameter> parameterList)
        {
            // TODO: cache function.
            var activation = TypeAnalyzer.GetActivation(type, CreateActivation);

            return new ActivateToInstantiate(activation, parameterList);
        }
        private IActivation CreateActivation(Type type)
        {
            return codeBuilder.CreateActivation(type);
        }

        public IInjection GetFieldInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            // TODO: cache function.
            var infusion = TypeAnalyzer.GetFieldInjection(type, CreateFieldInfusion);

            return new InfuseToInject(infusion, parameterList);
        }
        private IInfusion CreateFieldInfusion(Type type)
        {
            return codeBuilder.CreateFieldInfusion(type);
        }

        public IInjection GetPropertyInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            // TODO: cache function.
            var infusion = TypeAnalyzer.GetPropertyInjection(type, CreatePropertyInfusion);

            return new InfuseToInject(infusion, parameterList);
        }
        private IInfusion CreatePropertyInfusion(Type type)
        {
            return codeBuilder.CreatePropertyInfusion(type);
        }

        public IInjection GetMethodInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            // TODO: cache function.
            var infusion = TypeAnalyzer.GetMethodInjection(type, CreateMethodInfusion);

            return new InfuseToInject(infusion, parameterList);
        }
        private IInfusion CreateMethodInfusion(Type type)
        {
            return codeBuilder.CreateMethodInfusion(type);
        }

        /// <inheritdoc/>
        public IEngine Build(IEnumerable<IDescription> descriptionList)
        {
            using var converter = new ConvertDescriptionListToEngine(codeBuilder, descriptionList);

            var engine = converter.Convert(out var registrationList);

            TypeAnalyzer.Validate(registrationList, engine);

            return engine;
        }
    }
}
