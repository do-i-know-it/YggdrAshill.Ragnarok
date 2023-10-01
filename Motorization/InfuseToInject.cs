using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class InfuseToInject :
        IInjection
    {
        private readonly IInfusion infusion;
        private readonly IReadOnlyList<IParameter> parameterList;

        public InfuseToInject(IInfusion infusion, IReadOnlyList<IParameter> parameterList)
        {
            this.infusion = infusion;
            this.parameterList = parameterList;
        }

        public void Inject(IResolver resolver, object instance)
        {
            var argumentList = infusion.ArgumentList;

            // TODO: object pooling.
            var instanceList = new object[argumentList.Count];

            for (var index = 0; index < argumentList.Count; index++)
            {
                var argument = argumentList[index];

                instanceList[index] = resolver.Resolve(parameterList, argument);
            }

            infusion.Infuse(instance, instanceList);
        }
    }
}