using System;
using System.Reflection;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    // TODO: rename class?
    public sealed class ConstructorInjection
    {
        public Type ImplementedType { get; }
        public ConstructorInfo Constructor { get; }
        public ParameterInfo[] ParameterList { get; }

        public ConstructorInjection(Type implementedType, ConstructorInfo constructor)
        {
            ImplementedType = implementedType;
            Constructor = constructor;
            ParameterList = Constructor.GetParameters();
        }
    }
}
