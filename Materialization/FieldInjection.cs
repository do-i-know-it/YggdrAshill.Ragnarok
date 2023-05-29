using System;
using System.Linq;
using System.Reflection;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    // TODO: rename class?
    public sealed class FieldInjection
    {
        public Type ImplementedType { get; }

        private readonly FieldInfo[] fieldList;
        public FieldInfo[] FieldList => fieldList.ToArray();

        public FieldInjection(Type implementedType, FieldInfo[] fieldList)
        {
            ImplementedType = implementedType;
            this.fieldList = fieldList;
        }
    }
}