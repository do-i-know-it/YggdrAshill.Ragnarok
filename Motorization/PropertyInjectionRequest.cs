using System;
using System.Reflection;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class PropertyInjectionRequest
    {
        public Type ImplementedType { get; }

        public PropertyInfo[] PropertyList { get; }

        public PropertyInjectionRequest(Type implementedType, PropertyInfo[] propertyList)
        {
            ImplementedType = implementedType;
            PropertyList = propertyList;
        }
    }
}
