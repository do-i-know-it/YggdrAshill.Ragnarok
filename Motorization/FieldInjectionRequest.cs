using System;
using System.Reflection;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class FieldInjectionRequest
    {
        public Type ImplementedType { get; }

        public FieldInfo[] FieldList { get; }

        public FieldInjectionRequest(Type implementedType, FieldInfo[] fieldList)
        {
            ImplementedType = implementedType;
            FieldList = fieldList;
        }
    }
}
