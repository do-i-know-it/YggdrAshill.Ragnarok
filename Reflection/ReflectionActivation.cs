namespace YggdrAshill.Ragnarok
{
    internal sealed class ReflectionActivation : IActivation
    {
        private readonly ConstructorInjectionRequest request;

        public ReflectionActivation(ConstructorInjectionRequest request)
        {
            this.request = request;
        }

        public IDependency Dependency => request.Dependency;

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
