﻿using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation of <see cref="IEngineContext"/> using <see cref="ISelector"/> and <see cref="ISolver"/>.
    /// </summary>
    public sealed class EngineContext :
        IEngineContext
    {
        private readonly ISelector selector;
        private readonly ISolver solver;

        public EngineContext(ISelector selector, ISolver solver)
        {
            this.selector = selector;
            this.solver = solver;

            activation = CreateActivation;
            fieldInfusion = CreateFieldInfusion;
            propertyInfusion = CreatePropertyInfusion;
            methodInfusion = CreateMethodInfusion;
        }

        private readonly Func<Type, IActivation> activation;
        private readonly Func<Type, IInfusion> fieldInfusion;
        private readonly Func<Type, IInfusion> propertyInfusion;
        private readonly Func<Type, IInfusion> methodInfusion;

        private readonly TypeAnalyzer typeAnalyzer = new TypeAnalyzer();

        /// <inheritdoc/>
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

            var injection = selector.CreateConstructorInjection(type);

            return solver.CreateActivation(injection);
        }

        /// <inheritdoc/>
        public IInfusion GetFieldInfusion(Type type)
        {
            return typeAnalyzer.GetFieldInfusion(type, fieldInfusion);
        }
        private IInfusion CreateFieldInfusion(Type type)
        {
            var injection = selector.CreateFieldInjection(type);

            return solver.CreateFieldInfusion(injection);
        }

        /// <inheritdoc/>
        public IInfusion GetPropertyInfusion(Type type)
        {
            return typeAnalyzer.GetPropertyInfusion(type, propertyInfusion);
        }
        private IInfusion CreatePropertyInfusion(Type type)
        {
            var injection = selector.CreatePropertyInjection(type);

            return solver.CreatePropertyInfusion(injection);
        }

        /// <inheritdoc/>
        public IInfusion GetMethodInfusion(Type type)
        {
            return typeAnalyzer.GetMethodInfusion(type, methodInfusion);
        }
        private IInfusion CreateMethodInfusion(Type type)
        {
            var injection = selector.CreateMethodInjection(type);

            return solver.CreateMethodInfusion(injection);
        }

        /// <inheritdoc/>
        public IEngine Build(IReadOnlyList<IDescription> descriptionList)
        {
            using var factory = new EngineFactory(this, descriptionList);

            var engine = factory.Create();

            typeAnalyzer.Validate(descriptionList, engine);

            return engine;
        }
    }
}
