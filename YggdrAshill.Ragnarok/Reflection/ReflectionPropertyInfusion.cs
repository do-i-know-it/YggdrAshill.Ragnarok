using YggdrAshill.Ragnarok.Materialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ReflectionPropertyInfusion :
        IInfusion
    {
        private readonly PropertyInfo[] propertyList;

        public IReadOnlyList<Argument> ArgumentList { get; }

        public ReflectionPropertyInfusion(PropertyInjection injection)
        {
            propertyList = injection.PropertyList;

            ArgumentList = propertyList.Select(info => new Argument(info.Name, info.PropertyType)).ToArray();
        }

        public void Infuse(object instance, object[] parameterList)
        {
            if (propertyList.Length == 0)
            {
                return;
            }

            if (propertyList.Length != parameterList.Length)
            {
                throw new ArgumentException();
            }

            for (var index = 0; index < propertyList.Length; index++)
            {
                var property = propertyList[index];
                var parameter = parameterList[index];

                var fieldType = property.PropertyType;
                var parameterType = parameter.GetType();

                // TODO: Type.IsInstanceOfType(object)?
                if (!fieldType.IsAssignableFrom(parameterType))
                {
                    throw new Exception();
                }

                property.SetValue(instance, parameter);
            }
        }
    }
}
