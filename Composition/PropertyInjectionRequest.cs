using System;
using System.Linq;
using System.Reflection;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class PropertyInjectionRequest
    {
        public Type ImplementedType { get; }

        private readonly PropertyInfo[] propertyList;
        public PropertyInfo[] PropertyList => propertyList.ToArray();

        public PropertyInjectionRequest(Type implementedType, PropertyInfo[] propertyList)
        {
            ImplementedType = implementedType;
            this.propertyList = propertyList;
        }
    }
}
