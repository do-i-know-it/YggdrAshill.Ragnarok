using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class CollectionActivation : IActivation
    {
        private readonly Type elementType;

        public CollectionActivation(Type elementType)
        {
            this.elementType = elementType;
        }

        public object Activate(object[] parameterList)
        {
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
