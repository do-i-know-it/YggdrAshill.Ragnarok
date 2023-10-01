using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ReflectionActivation : IActivation
    {
        private readonly DependencyInjectionRequest request;

        public IReadOnlyList<Argument> ArgumentList
            => request.ParameterList.Select(info => new Argument(info.Name, info.ParameterType)).ToArray();

        public ReflectionActivation(DependencyInjectionRequest request)
        {
            this.request = request;
        }

        public object Activate(object[] parameterList)
        {
            var constructor = request.Constructor;
            var argumentList = request.ParameterList;

            if (argumentList.Length != parameterList.Length)
            {
                throw new RagnarokReflectionException(request.ImplementedType, nameof(parameterList));
            }

            for (var index = 0; index < argumentList.Length; index++)
            {
                var argumentType = argumentList[index].ParameterType;
                var parameterType = parameterList[index].GetType();

                // TODO: Type.IsInstanceOfType(object)?
                if (!argumentType.IsAssignableFrom(parameterType))
                {
                    throw new RagnarokReflectionException(parameterType, $"{parameterType} is not assignable from {argumentType}.");
                }
            }

            return constructor.Invoke(parameterList);
        }
    }
}
