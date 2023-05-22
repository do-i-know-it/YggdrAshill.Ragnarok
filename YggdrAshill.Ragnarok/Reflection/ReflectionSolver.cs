using YggdrAshill.Ragnarok.Hierarchization;
using YggdrAshill.Ragnarok.Materialization;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Reflection
{
    /// <summary>
    /// Implementation <see cref="ISolver"/> with Reflection.
    /// </summary>
    public sealed class ReflectionSolver :
        ISolver
    {
        public static ReflectionSolver Instance { get; } = new ReflectionSolver();

        private ReflectionSolver()
        {

        }

        public IActivation CreateActivation(ConstructorInjection injection)
        {
            return new ReflectionActivation(injection);
        }

        public IInfusion CreateFieldInfusion(FieldInjection injection)
        {
            return new ReflectionFieldInfusion(injection);
        }

        public IInfusion CreatePropertyInfusion(PropertyInjection injection)
        {
            return new ReflectionPropertyInfusion(injection);
        }

        public IInfusion CreateMethodInfusion(MethodInjection injection)
        {
            return new ReflectionMethodInfusion(injection);
        }

        public IActivation CreateCollectionActivation(Type elementType)
        {
            return new CollectionGeneration(elementType);
        }

        private sealed class CollectionGeneration :
            IActivation
        {
            private readonly Type elementType;

            public CollectionGeneration(Type elementType)
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
                        throw new Exception();
                    }

                    array.SetValue(parameter, index);
                }

                return array;
            }
        }
    }
}
