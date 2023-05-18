using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Hierarchization;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Materialization
{
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

        public IActivation CreateActivation(Type type)
        {
            if (type.IsArray)
            {
                return solver.CreateCollectionActivation(type.GetElementType()!);
            }

            var injection = selector.CreateConstructorInjection(type);

            return solver.CreateActivation(injection);
        }

        public IInfusion CreateFieldInfusion(Type type)
        {
            var injection = selector.CreateFieldInjection(type);

            return solver.CreateFieldInfusion(injection);
        }

        public IInfusion CreatePropertyInfusion(Type type)
        {
            var injection = selector.CreatePropertyInjection(type);

            return solver.CreatePropertyInfusion(injection);
        }

        public IInfusion CreateMethodInfusion(Type type)
        {
            var injection = selector.CreateMethodInjection(type);

            return solver.CreateMethodInfusion(injection);
        }

        public IRegistry Build(IEnumerable<IDescription> descriptionList, out IEnumerable<IRegistration> registrationList)
        {
            using (var converter = new ConvertDescriptionListToEngine(this, descriptionList))
            {
                return converter.Convert(out registrationList);
            }
        }
    }
}
