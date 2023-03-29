using YggdrAshill.Ragnarok.Hierarchization;
using YggdrAshill.Ragnarok.Materialization;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Reflection
{
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

        public ICollectionGeneration CreateCollectionGeneration(Type type)
        {
            return new CollectionGeneration(type);
        }

        private sealed class CollectionGeneration :
            ICollectionGeneration
        {
            public Type ElementType { get; }

            public CollectionGeneration(Type elementType)
            {
                ElementType = elementType;
            }

            public object Create(IScopedResolver resolver, IReadOnlyList<IRegistration> registrationList)
            {
                var array = Array.CreateInstance(ElementType, registrationList.Count);

                for (var index = 0; index < registrationList.Count; index++)
                {
                    array.SetValue(resolver.Resolve(registrationList[index]), index);
                }

                return array;
            }
        }
    }
}
