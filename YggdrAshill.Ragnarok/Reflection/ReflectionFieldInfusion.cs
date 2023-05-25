using YggdrAshill.Ragnarok.Materialization;
using System;
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
                // TODO: throw original exception.
                throw new ArgumentException($"{instance} is not {implementedType}.");
            }
            if (fieldList.Length != parameterList.Length)
            {
                // TODO: throw original exception.
                throw new ArgumentException(nameof(parameterList));
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
                    // TODO: throw original exception.
                    throw new ArgumentException($"{parameterType} is not assignable from {fieldType}.");
                }

                field.SetValue(instance, parameter);
            }
        }
    }
}
