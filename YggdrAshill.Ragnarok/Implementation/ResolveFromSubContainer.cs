using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ResolveFromSubContainer : IInstantiation
    {
        private readonly Type type;
        private readonly IObjectResolver other;

        public ResolveFromSubContainer(Type type, IObjectResolver other)
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
