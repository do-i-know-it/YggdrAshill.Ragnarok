using System;
using System.Reflection;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class MethodInjectionRequest
    {
        public Type ImplementedType { get; }
        public MethodInfo Method { get; }
        public ParameterInfo[] ParameterList { get; }

        public MethodInjectionRequest(Type implementedType, MethodInfo method)
        {
            ImplementedType = implementedType;
            Method = method;
            ParameterList = Method.GetParameters();
        }
    }
}
