using System;
using System.Linq;
using System.Reflection;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class FieldInjectionRequest
    {
        public Type ImplementedType { get; }

        private readonly FieldInfo[] fieldList;
        public FieldInfo[] FieldList => fieldList.ToArray();

        public FieldInjectionRequest(Type implementedType, FieldInfo[] fieldList)
        {
            ImplementedType = implementedType;
            this.fieldList = fieldList;
        }
    }
}
