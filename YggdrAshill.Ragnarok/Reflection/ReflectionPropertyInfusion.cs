using YggdrAshill.Ragnarok.Materialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ReflectionPropertyInfusion :
        IInfusion
    {
        private readonly PropertyInjection injection;

        public IReadOnlyList<Argument> ArgumentList
            => injection.PropertyList.Select(info => new Argument(info.Name, info.PropertyType)).ToArray();

        public ReflectionPropertyInfusion(PropertyInjection injection)
        {
            this.injection = injection;
        }

        public void Infuse(object instance, object[] parameterList)
        {
            var implementedType = injection.ImplementedType;
            var propertyList = injection.PropertyList;

            if (!implementedType.IsInstanceOfType(instance))
            {
                // TODO: throw original exception.
                throw new ArgumentException($"{instance} is not {implementedType}.");
            }
            if (propertyList.Length != parameterList.Length)
            {
                // TODO: throw original exception.
                throw new ArgumentException(nameof(parameterList));
            }

            for (var index = 0; index < propertyList.Length; index++)
            {
                var property = propertyList[index];
                var parameter = parameterList[index];

                var propertyType = property.PropertyType;
                var parameterType = parameter.GetType();

                // TODO: Type.IsInstanceOfType(object)?
                if (!propertyType.IsAssignableFrom(parameterType))
                {
                    // TODO: throw original exception.
                    throw new ArgumentException($"{parameterType} is not assignable from {propertyType}.");
                }

                property.SetValue(instance, parameter);
            }
        }
    }
}
