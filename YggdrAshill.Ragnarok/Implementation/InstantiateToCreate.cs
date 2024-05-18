using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class InstantiateToCreate<T> : IInstantiation
        where T : notnull
    {
        private readonly Func<T> onCreated;

        public InstantiateToCreate(Func<T> onCreated)
        {
            this.onCreated = onCreated;
        }

        public object Instantiate(IObjectResolver resolver)
        {
            return onCreated.Invoke();
        }
    }
}
