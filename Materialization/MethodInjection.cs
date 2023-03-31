using System.Reflection;

namespace YggdrAshill.Ragnarok.Materialization
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
