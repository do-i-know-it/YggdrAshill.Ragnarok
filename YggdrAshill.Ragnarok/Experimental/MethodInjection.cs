using System.Reflection;

namespace YggdrAshill.Ragnarok
{
    public sealed class MethodInjection
    {
        public MethodInfo Method { get; }
        public ParameterInfo[] ParameterList => Method.GetParameters();

        public MethodInjection(MethodInfo method)
        {
            Method = method;
        }
    }
}
