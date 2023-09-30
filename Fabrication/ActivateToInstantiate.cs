using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ActivateToInstantiate : IInstantiationV2
    {
        private readonly IActivation activation;
        private readonly IReadOnlyList<IParameter> parameterList;

        public ActivateToInstantiate(IActivation activation, IReadOnlyList<IParameter> parameterList)
        {
            this.activation = activation;
            this.parameterList = parameterList;
        }

        public object Instantiate(IObjectResolver resolver)
        {
            var argumentList = activation.ArgumentList;

            // TODO: object pooling.
            var instanceList = new object[argumentList.Count];

            for (var index = 0; index < argumentList.Count; index++)
            {
                var argument = argumentList[index];

                instanceList[index] = resolver.Resolve(parameterList, argument);
            }

            return activation.Activate(instanceList);
        }
    }
}
