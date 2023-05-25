using YggdrAshill.Ragnarok.Materialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ReflectionMethodInfusion :
        IInfusion
    {
        private readonly MethodInjection injection;

        public IReadOnlyList<Argument> ArgumentList
            => injection.ParameterList.Select(info => new Argument(info.Name, info.ParameterType)).ToArray();

        public ReflectionMethodInfusion(MethodInjection injection)
        {
            this.injection = injection;
        }

        public void Infuse(object instance, object[] parameterList)
        {
            var implementedType = injection.ImplementedType;
            var method = injection.Method;
            var argumentList = injection.ParameterList;

            if (!implementedType.IsInstanceOfType(instance))
            {
                // TODO: throw original exception.
                throw new ArgumentException($"{instance} is not {implementedType}.");
            }
            if (argumentList.Length != parameterList.Length)
            {
                // TODO: throw original exception.
                throw new ArgumentException(nameof(parameterList));
            }

            for (var index = 0; index < argumentList.Length; index++)
            {
                var argumentType = argumentList[index].ParameterType;
                var parameterType = parameterList[index].GetType();

                // TODO: Type.IsInstanceOfType(object)?
                if (!argumentType.IsAssignableFrom(parameterType))
                {
                    // TODO: throw original exception.
                    throw new ArgumentException($"{parameterType} is not assignable from {argumentType}.");
                }
            }

            method.Invoke(instance, parameterList);
        }
    }
}
