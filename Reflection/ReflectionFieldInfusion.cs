using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ReflectionFieldInfusion : IInfusion
    {
        private readonly FieldInjectionRequest request;

        public IReadOnlyList<Argument> ArgumentList
            => request.FieldList.Select(info => new Argument(info.Name, info.FieldType)).ToArray();

        public ReflectionFieldInfusion(FieldInjectionRequest request)
        {
            this.request = request;
        }

        public void Infuse(object instance, object[] parameterList)
        {
            var implementedType = request.ImplementedType;
            var fieldList = request.FieldList;

            if (!implementedType.IsInstanceOfType(instance))
            {
                throw new RagnarokReflectionException(implementedType, $"{instance} is not {implementedType}.");
            }
            if (fieldList.Length != parameterList.Length)
            {
                throw new RagnarokReflectionException(implementedType, nameof(parameterList));
            }

            for (var index = 0; index < fieldList.Length; index++)
            {
                var field = fieldList[index];
                var parameter = parameterList[index];

                var fieldType = field.FieldType;
                var parameterType = parameter.GetType();

                // TODO: Type.IsInstanceOfType(object)?
                if (!fieldType.IsAssignableFrom(parameterType))
                {
                    throw new RagnarokReflectionException(parameterType, $"{parameterType} is not assignable from {fieldType}.");
                }

                field.SetValue(instance, parameter);
            }
        }
    }
}
