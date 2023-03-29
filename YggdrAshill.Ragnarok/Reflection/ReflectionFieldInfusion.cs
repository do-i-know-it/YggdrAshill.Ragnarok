using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Materialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace YggdrAshill.Ragnarok.Reflection
{
    internal sealed class ReflectionFieldInfusion :
        IInfusion
    {
        private readonly FieldInfo[] fieldList;
        public IReadOnlyList<Type> DependentTypeList { get; }

        public ReflectionFieldInfusion(FieldInjection injection)
        {
            fieldList = injection.FieldList;
            DependentTypeList
                = fieldList.Select(field =>  field.FieldType).Distinct().ToArray();
        }

        public void Infuse(object instance, IResolver resolver, IReadOnlyList<IParameter> parameterList)
        {
            if (fieldList.Length == 0)
            {
                return;
            }

            foreach (var field in fieldList)
            {
                var value = resolver.Resolve(parameterList, field.FieldType, field.Name);
                field.SetValue(instance, value);
            }
        }
    }
}
