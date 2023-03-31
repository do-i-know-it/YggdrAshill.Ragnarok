using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Materialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace YggdrAshill.Ragnarok.Reflection
{
    internal sealed class ReflectionActivation :
        IActivation
    {
        private readonly ConstructorInfo constructor;
        private readonly ParameterInfo[] argumentList;

        public IReadOnlyList<Type> DependentTypeList { get; }

        public ReflectionActivation(ConstructorInjection injection)
        {
            constructor = injection.Constructor;
            argumentList = injection.ParameterList;

            DependentTypeList
                = argumentList.Select(parameter => parameter.ParameterType).Distinct().ToArray();
        }

        public object Activate(IResolver resolver, IReadOnlyList<IParameter> parameterList)
        {
            // TODO: object pooling.
            var parameterValueList = new object[argumentList.Length];

            for (var index = 0; index < argumentList.Length; index++)
            {
                var parameter = argumentList[index];

                parameterValueList[index] = resolver.Resolve(parameterList, parameter.ParameterType, parameter.Name);
            }

            return constructor.Invoke(parameterValueList);
        }
    }
}
