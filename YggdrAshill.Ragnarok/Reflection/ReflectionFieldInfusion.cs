using YggdrAshill.Ragnarok.Materialization;
using YggdrAshill.Ragnarok.Memorization;
using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ReflectionFieldInfusion :
        IInfusion
    {
        private readonly FieldInjection injection;

        public IReadOnlyList<Argument> ArgumentList
            => injection.FieldList.Select(info => new Argument(info.Name, info.FieldType)).ToArray();

        public ReflectionFieldInfusion(FieldInjection injection)
        {
            this.injection = injection;
        }

        public void Infuse(object instance, object[] parameterList)
        {
            var implementedType = injection.ImplementedType;
            var fieldList = injection.FieldList;

            if (!implementedType.IsInstanceOfType(instance))
            {
                throw new RagnarokArgumentException(implementedType, $"{instance} is not {implementedType}.");
            }
            if (fieldList.Length != parameterList.Length)
            {
                throw new RagnarokArgumentException(implementedType, nameof(parameterList));
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
                    throw new RagnarokArgumentException(parameterType, $"{parameterType} is not assignable from {fieldType}.");
                }

                field.SetValue(instance, parameter);
            }
        }
    }
}
