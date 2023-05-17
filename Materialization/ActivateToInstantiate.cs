using YggdrAshill.Ragnarok.Construction;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Materialization
{
    internal sealed class ActivateToInstantiate :
        IInstantiation
    {
        private readonly IActivation activation;
        private readonly IReadOnlyList<IParameter> parameterList;

        public ActivateToInstantiate(IActivation activation, IReadOnlyList<IParameter> parameterList)
        {
            this.activation = activation;
            this.parameterList = parameterList;
        }

        public object Instantiate(IResolver resolver)
        {
            var argumentList = activation.ArgumentList;

            // TODO: object pooling.
            var instanceList = new object[argumentList.Count];

            for (var index = 0; index < argumentList.Count; index++)
            {
                var argument = argumentList[index];

                instanceList[index] = resolver.Resolve(parameterList, argument.Type, argument.Name);
            }

            return activation.Activate(instanceList);
        }
    }
}
