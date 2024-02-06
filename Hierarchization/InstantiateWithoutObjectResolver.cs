using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class InstantiateWithoutObjectResolver : IInstantiation
    {
        private readonly IActivation activation;

        public InstantiateWithoutObjectResolver(IActivation activation)
        {
            this.activation = activation;
        }

        public object Instantiate(IObjectResolver resolver)
        {
            return activation.Activate(Array.Empty<object>());
        }
    }
}
