using System;
using System.Linq;
using System.Reflection;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    // TODO: rename class?
    public sealed class PropertyInjection
    {
        public Type ImplementedType { get; }

        private readonly PropertyInfo[] propertyList;
        public PropertyInfo[] PropertyList => propertyList.ToArray();

        public PropertyInjection(Type implementedType, PropertyInfo[] propertyList)
        {
            ImplementedType = implementedType;
            this.propertyList = propertyList;
        }
    }
}
