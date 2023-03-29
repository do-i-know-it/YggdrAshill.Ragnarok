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
        private readonly ParameterInfo[] parameterInformationList;
        public IReadOnlyList<Type> DependentTypeList { get; }

        public ReflectionMethodInfusion(MethodInjection injection)
        {
            method = injection.Method;
            parameterInformationList = injection.ParameterList;

            DependentTypeList
                = parameterInformationList.Select(parameter => parameter.ParameterType).Distinct().ToArray();
        }

        public void Infuse(object instance, IResolver resolver, IReadOnlyList<IParameter> parameterList)
        {
            // TODO: object pooling.
            var parameterValueList = new object[parameterInformationList.Length];

            for (var index = 0; index < parameterInformationList.Length; index++)
            {
                var parameter = parameterInformationList[index];

                parameterValueList[index] = resolver.Resolve(parameterList, parameter.ParameterType, parameter.Name);
            }

            method.Invoke(instance, parameterValueList);
        }
    }
}
