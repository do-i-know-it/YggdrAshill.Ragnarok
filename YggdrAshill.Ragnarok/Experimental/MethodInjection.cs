using System;
using System.Reflection;

namespace YggdrAshill.Ragnarok.Memorization
{
    // TODO: add document comments.
    // TODO: rename class?
    public sealed class MethodInjection
    {
        public Type ImplementedType { get; }
        public MethodInfo Method { get; }
        public ParameterInfo[] ParameterList { get; }

        public MethodInjection(Type implementedType, MethodInfo method)
        {
            ImplementedType = implementedType;
            Method = method;
            ParameterList = Method.GetParameters();
        }
    }
}
