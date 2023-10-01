using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class InstantiateFromOtherResolver : IInstantiation
    {
        private readonly Type assignedType;
        private readonly IObjectResolver other;

        public InstantiateFromOtherResolver(Type assignedType, IObjectResolver other)
        {
            this.assignedType = assignedType;
            this.other = other;
        }

        public object Instantiate(IObjectResolver resolver)
        {
            return other.Resolve(assignedType);
        }
    }
}
