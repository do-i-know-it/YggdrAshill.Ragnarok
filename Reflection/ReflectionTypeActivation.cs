using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ReflectionTypeActivation : IActivation
    {
        private readonly Type implementedType;

        public ReflectionTypeActivation(Type implementedType)
        {
            this.implementedType = implementedType;
        }

        public object Activate(object[] parameterList)
        {
            return Activator.CreateInstance(implementedType);
        }
    }
}
