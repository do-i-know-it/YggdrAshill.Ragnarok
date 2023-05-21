using YggdrAshill.Ragnarok.Construction;
using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class InstantiateWithFunction :
        IInstantiation
    {
        private readonly Func<object> onInstantiated;

        public InstantiateWithFunction(Func<object> onInstantiated)
        {
            this.onInstantiated = onInstantiated;
        }

        public object Instantiate(IResolver resolver)
        {
            return onInstantiated.Invoke();
        }
    }
}
