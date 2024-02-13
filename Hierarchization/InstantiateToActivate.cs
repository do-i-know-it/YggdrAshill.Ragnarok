using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class InstantiateToActivate : IInstantiation
    {
        private readonly IActivation activation;

        public InstantiateToActivate(IActivation activation)
        {
            this.activation = activation;
        }

        public object Instantiate(IObjectResolver resolver)
        {
            return activation.Activate(Array.Empty<object>());
        }
    }
}
