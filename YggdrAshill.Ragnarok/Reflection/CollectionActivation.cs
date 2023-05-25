using YggdrAshill.Ragnarok.Materialization;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class CollectionActivation :
        IActivation
    {
        private readonly Type elementType;

        public CollectionActivation(Type elementType)
        {
            this.elementType = elementType;
        }

        public IReadOnlyList<Argument> ArgumentList { get; } = Array.Empty<Argument>();

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
                    // TODO: throw original exception.
                    throw new ArgumentException($"{parameterType} is not assignable from {elementType}.");
                }

                array.SetValue(parameter, index);
            }

            return array;
        }
    }
}
