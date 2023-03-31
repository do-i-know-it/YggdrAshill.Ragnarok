using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Motorization;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Materialization
{
    /// <summary>
    /// Implementation of <see cref="IEngineBuilder"/> with <see cref="ISelector"/> and <see cref="ISolver"/>.
    /// </summary>
    public sealed class EngineBuilder :
        IEngineBuilder
    {
        private readonly ISelector selector;
        private readonly ISolver solver;

        /// <summary>
        /// Constructor of <see cref="EngineBuilder"/>.
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="solver"></param>
        public EngineBuilder(ISelector selector, ISolver solver)
        {
            this.selector = selector;
            this.solver = solver;
        }

        public IInstantiation GetInstantiation(Type type, IReadOnlyList<IParameter> parameterList)
        {
            var activation = TypeAnalyzer.GetActivation(type, CreateActivation);

            return new ActivateToInstantiate(activation, parameterList);
        }
        private IActivation CreateActivation(Type type)
        {
            var injection = selector.CreateConstructorInjection(type);

            return solver.CreateActivation(injection);
        }

        public IInjection GetFieldInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            var infusion = TypeAnalyzer.GetFieldInjection(type, CreateFieldInfusion);

            return new InfuseToInject(infusion, parameterList);
        }
        private IInfusion CreateFieldInfusion(Type type)
        {
            var injection = selector.CreateFieldInjection(type);

            return solver.CreateFieldInfusion(injection);
        }

        public IInjection GetPropertyInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            var infusion = TypeAnalyzer.GetPropertyInjection(type, CreatePropertyInfusion);

            return new InfuseToInject(infusion, parameterList);
        }
        private IInfusion CreatePropertyInfusion(Type type)
        {
            var injection = selector.CreatePropertyInjection(type);

            return solver.CreatePropertyInfusion(injection);
        }

        public IInjection GetMethodInjection(Type type, IReadOnlyList<IParameter> parameterList)
        {
            var infusion = TypeAnalyzer.GetMethodInjection(type, CreateMethodInfusion);

            return new InfuseToInject(infusion, parameterList);
        }
        private IInfusion CreateMethodInfusion(Type type)
        {
            var injection = selector.CreateMethodInjection(type);

            return solver.CreateMethodInfusion(injection);
        }

        /// <inheritdoc/>
        public IEngine Build(IEnumerable<IDescription> descriptionList)
        {
            using var converter = new ConvertDescriptionListToEngine(solver, descriptionList);

            var engine = converter.Convert(out var registrationList);

            TypeAnalyzer.Validate(registrationList, engine);

            return engine;
        }
    }
}
