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
            return activation.Activate(resolver, parameterList);
        }
    }
}
