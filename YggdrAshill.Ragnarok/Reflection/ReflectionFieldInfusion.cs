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

        public IReadOnlyList<Argument> ArgumentList { get; }
        public IReadOnlyList<Type> DependentTypeList { get; }

        public ReflectionFieldInfusion(FieldInjection injection)
        {
            fieldList = injection.FieldList;

            ArgumentList = fieldList.Select(info => new Argument(info.Name, info.FieldType)).ToArray();
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
