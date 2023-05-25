using System.Reflection;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    // TODO: rename class?
    public sealed class ConstructorInjection
    {
        public ConstructorInfo Constructor { get; }
        public ParameterInfo[] ParameterList => Constructor.GetParameters();

        public ConstructorInjection(ConstructorInfo constructor)
        {
            Constructor = constructor;
        }
    }
}
