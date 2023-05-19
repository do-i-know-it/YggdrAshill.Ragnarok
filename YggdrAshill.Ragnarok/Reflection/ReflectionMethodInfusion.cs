using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Materialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace YggdrAshill.Ragnarok.Reflection
{
    internal sealed class ReflectionMethodInfusion :
        IInfusion
    {
        private readonly MethodInfo method;
        private readonly ParameterInfo[] argumentList;

        public IReadOnlyList<Argument> ArgumentList { get; }
        public IReadOnlyList<Type> DependentTypeList { get; }

        public ReflectionMethodInfusion(MethodInjection injection)
        {
            method = injection.Method;
            argumentList = injection.ParameterList;

            ArgumentList = argumentList.Select(info => new Argument(info.Name, info.ParameterType)).ToArray();

            DependentTypeList
                = argumentList.Select(parameter => parameter.ParameterType).Distinct().ToArray();
        }

        public void Infuse(object instance, IResolver resolver, IReadOnlyList<IParameter> parameterList)
        {
            // TODO: object pooling.
            var parameterValueList = new object[argumentList.Length];

            for (var index = 0; index < argumentList.Length; index++)
            {
                var parameter = argumentList[index];

                parameterValueList[index] = resolver.Resolve(parameterList, parameter.ParameterType, parameter.Name);
            }

            method.Invoke(instance, parameterValueList);
        }

        public void Infuse(object instance, object[] parameterList)
        {
            if (argumentList.Length != parameterList.Length)
            {
                throw new ArgumentException();
            }

            for (var index = 0; index < argumentList.Length; index++)
            {
                var argumentType = argumentList[index].ParameterType;
                var parameterType = parameterList[index].GetType();

                // TODO: Type.IsInstanceOfType(object)?
                if (!argumentType.IsAssignableFrom(parameterType))
                {
                    throw new Exception();
                }
            }
            method.Invoke(instance, parameterList);
        }
    }
}
