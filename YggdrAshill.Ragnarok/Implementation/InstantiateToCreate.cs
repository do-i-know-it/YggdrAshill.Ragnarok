using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class InstantiateToCreate<T> : IInstantiation
        where T : notnull
    {
        private readonly Func<T> onInstantiated;

        public InstantiateToCreate(Func<T> onInstantiated)
        {
            this.onInstantiated = onInstantiated;
        }

        public object Instantiate(IObjectResolver resolver)
        {
            return onInstantiated.Invoke();
        }
    }
}
