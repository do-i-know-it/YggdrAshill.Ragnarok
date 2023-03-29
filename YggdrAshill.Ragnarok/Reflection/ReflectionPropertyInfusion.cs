using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Materialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace YggdrAshill.Ragnarok.Reflection
{
    internal sealed class ReflectionPropertyInfusion :
        IInfusion
    {
        private readonly PropertyInfo[] propertyList;
        public IReadOnlyList<Type> DependentTypeList { get; }

        public ReflectionPropertyInfusion(PropertyInjection injection)
        {
            propertyList = injection.PropertyList;
            DependentTypeList
                = propertyList.Select(field =>  field.PropertyType).Distinct().ToArray();
        }

        public void Infuse(object instance, IResolver resolver, IReadOnlyList<IParameter> parameterList)
        {
            if (propertyList.Length == 0)
            {
                return;
            }

            foreach (var field in propertyList)
            {
                var value = resolver.Resolve(parameterList, field.PropertyType, field.Name);
                field.SetValue(instance, value);
            }
        }
    }
}
