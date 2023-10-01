using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ReflectionPropertyInfusion : IInfusion
    {
        private readonly PropertyInjectionRequest request;

        public IReadOnlyList<Argument> ArgumentList
            => request.PropertyList.Select(info => new Argument(info.Name, info.PropertyType)).ToArray();

        public ReflectionPropertyInfusion(PropertyInjectionRequest request)
        {
            this.request = request;
        }

        public void Infuse(object instance, object[] parameterList)
        {
            var implementedType = request.ImplementedType;
            var propertyList = request.PropertyList;

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
