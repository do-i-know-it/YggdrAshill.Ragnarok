using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ReflectionActivation :
        IActivation
    {
        private readonly ConstructorInjection injection;

        public IReadOnlyList<Argument> ArgumentList
            => injection.ParameterList.Select(info => new Argument(info.Name, info.ParameterType)).ToArray();

        public ReflectionActivation(ConstructorInjection injection)
        {
            this.injection = injection;
        }

        public object Activate(object[] parameterList)
        {
            var constructor = injection.Constructor;
            var argumentList = injection.ParameterList;

            if (argumentList.Length != parameterList.Length)
            {
                throw new RagnarokArgumentException(injection.ImplementedType, nameof(parameterList));
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

            return constructor.Invoke(parameterList);
        }
    }
}
