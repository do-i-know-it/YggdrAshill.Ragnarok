using System;
using System.Collections.Generic;
using System.Reflection;

namespace YggdrAshill.Ragnarok
{
    public interface IDependencyEnumeration
    {
        IEnumerable<ConstructorInfo> GetConstructorList(Type type);
        IEnumerable<FieldInfo> GetFieldList(Type type);
        IEnumerable<PropertyInfo> GetPropertyList(Type type);
        IEnumerable<MethodInfo> GetMethodList(Type type);
    }
}
