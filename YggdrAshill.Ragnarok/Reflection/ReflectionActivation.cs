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

        public IReadOnlyList<Argument> ArgumentList { get; }
        public IReadOnlyList<Type> DependentTypeList { get; }

        public ReflectionActivation(ConstructorInjection injection)
        {
            constructor = injection.Constructor;
            argumentList = injection.ParameterList;

            ArgumentList = argumentList.Select(info => new Argument(info.Name, info.ParameterType)).ToArray();

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

        public object Activate(object[] parameterList)
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

            return constructor.Invoke(parameterList);
        }
    }
}
