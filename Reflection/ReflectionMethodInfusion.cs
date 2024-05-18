namespace YggdrAshill.Ragnarok
{
    internal sealed class ReflectionMethodInfusion : IInfusion
    {
        private readonly MethodInjectionRequest request;

        public ReflectionMethodInfusion(MethodInjectionRequest request)
        {
            this.request = request;
        }

        public void Infuse(ref object instance, object[] parameterList)
        {
            var implementedType = request.ImplementedType;
            var method = request.Method;
            var argumentList = request.ParameterList;

            if (!implementedType.IsInstanceOfType(instance))
            {
                throw new RagnarokReflectionException(implementedType, $"{instance} is not {implementedType}.");
            }
            if (argumentList.Length != parameterList.Length)
            {
                throw new RagnarokReflectionException(implementedType, nameof(parameterList));
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

            method.Invoke(instance, parameterList);
        }
    }
}
