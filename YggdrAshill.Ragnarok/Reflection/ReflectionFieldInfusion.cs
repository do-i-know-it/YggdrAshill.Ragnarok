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

        public IReadOnlyList<Argument> ArgumentList { get; }

        public ReflectionFieldInfusion(FieldInjection injection)
        {
            fieldList = injection.FieldList;

            ArgumentList = fieldList.Select(info => new Argument(info.Name, info.FieldType)).ToArray();
        }

        public void Infuse(object instance, object[] parameterList)
        {
            if (fieldList.Length == 0)
            {
                return;
            }

            if (fieldList.Length != parameterList.Length)
            {
                throw new ArgumentException();
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
                    throw new Exception();
                }

                field.SetValue(instance, parameter);
            }
        }
    }
}
