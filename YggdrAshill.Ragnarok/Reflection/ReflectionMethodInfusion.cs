using YggdrAshill.Ragnarok.Materialization;
using YggdrAshill.Ragnarok.Memorization;
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
                throw new RagnarokArgumentException(implementedType, $"{instance} is not {implementedType}.");
            }
            if (argumentList.Length != parameterList.Length)
            {
                throw new RagnarokArgumentException(implementedType, nameof(parameterList));
            }

            for (var index = 0; index < argumentList.Length; index++)
            {
                var argumentType = argumentList[index].ParameterType;
                var parameterType = parameterList[index].GetType();

                // TODO: Type.IsInstanceOfType(object)?
                if (!argumentType.IsAssignableFrom(parameterType))
                {
                    throw new RagnarokArgumentException(parameterType, $"{parameterType} is not assignable from {argumentType}.");
                }
            }

            method.Invoke(instance, parameterList);
        }
    }
}
