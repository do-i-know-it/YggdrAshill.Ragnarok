using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class InstantiateFromSubContainer : IInstantiation
    {
        private readonly Type type;
        private readonly IObjectResolver other;

        public InstantiateFromSubContainer(Type type, IObjectResolver other)
        {
            this.type = type;
            this.other = other;
        }

        public object Instantiate(IObjectResolver resolver)
        {
            return other.Resolve(type);
        }
    }
}
