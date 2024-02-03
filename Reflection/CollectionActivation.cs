using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class CollectionActivation : IActivation
    {
        private readonly CollectionInjectionRequest request;

        public CollectionActivation(CollectionInjectionRequest request)
        {
            this.request = request;
        }

        public IDependency Dependency => request.Dependency;

        public object Activate(object[] parameterList)
        {
            var elementType = request.ElementType;
            var array = Array.CreateInstance(elementType, parameterList.Length);

            for (var index = 0; index < parameterList.Length; index++)
            {
                var parameter = parameterList[index];
                var parameterType = parameter.GetType();

                // TODO: Type.IsInstanceOfType(object)?
                if (!elementType.IsAssignableFrom(parameterType))
                {
                    throw new RagnarokReflectionException(parameterType, $"{parameterType} is not assignable from {elementType}.");
                }

                array.SetValue(parameter, index);
            }

            return array;
        }
    }
}
