using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class InstantiateWithoutObjectResolver : IInstantiation
    {
        private readonly IActivationV2 activation;

        public InstantiateWithoutObjectResolver(IActivationV2 activation)
        {
            this.activation = activation;
        }

        public object Instantiate(IObjectResolver resolver)
        {
            return activation.Activate(Array.Empty<object>());
        }
    }
}
