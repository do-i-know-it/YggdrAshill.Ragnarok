using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ReflectionPropertyInfusion : IInfusion
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
                throw new RagnarokReflectionException(implementedType, $"{instance} is not {implementedType}.");
            }
            if (propertyList.Length != parameterList.Length)
            {
                throw new RagnarokReflectionException(implementedType, nameof(parameterList));
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
                    throw new RagnarokReflectionException(implementedType, $"{parameterType} is not assignable from {propertyType}.");
                }

                property.SetValue(instance, parameter);
            }
        }
    }
}
