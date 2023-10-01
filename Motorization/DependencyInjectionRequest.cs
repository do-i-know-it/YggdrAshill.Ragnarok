using System;
using System.Reflection;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class DependencyInjectionRequest
    {
        public Type ImplementedType { get; }
        public ConstructorInfo Constructor { get; }
        public ParameterInfo[] ParameterList { get; }

        public DependencyInjectionRequest(Type implementedType, ConstructorInfo constructor)
        {
            ImplementedType = implementedType;
            Constructor = constructor;
            ParameterList = Constructor.GetParameters();
        }
    }
}
