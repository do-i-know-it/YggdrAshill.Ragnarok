using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class InstantiateInstance<T> : IInstantiation
        where T : notnull
    {
        private readonly Func<T> onInstantiated;

        public InstantiateInstance(Func<T> onInstantiated)
        {
            this.onInstantiated = onInstantiated;
        }

        public object Instantiate(IObjectResolver resolver)
        {
            return onInstantiated.Invoke();
        }
    }
}
