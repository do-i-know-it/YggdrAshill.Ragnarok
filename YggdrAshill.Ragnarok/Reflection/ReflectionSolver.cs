using YggdrAshill.Ragnarok.Hierarchization;
using YggdrAshill.Ragnarok.Materialization;
using System;
using System.Collections.Generic;
using YggdrAshill.Ragnarok.Construction;

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

        public ICollectionGeneration CreateCollectionGeneration(Type type)
        {
            return new CollectionGeneration(type);
        }

        public IActivation CreateCollectionActivation(Type elementType)
        {
            return new CollectionGeneration(elementType);
        }

        public Type GetLocalInstanceListType(Type elementType)
        {
            return typeof(LocalInstanceList<>).MakeGenericType(elementType);
        }

        private sealed class CollectionGeneration :
            ICollectionGeneration,
            IActivation
        {
            public Type ElementType { get; }

            public CollectionGeneration(Type elementType)
            {
                ElementType = elementType;
            }

            public IReadOnlyList<Argument> ArgumentList { get; } = Array.Empty<Argument>();
            public IReadOnlyList<Type> DependentTypeList { get; } = Array.Empty<Type>();

            public object Create(IScopedResolver resolver, IReadOnlyList<IRegistration> registrationList)
            {
                var array = Array.CreateInstance(ElementType, registrationList.Count);

                for (var index = 0; index < registrationList.Count; index++)
                {
                    array.SetValue(resolver.Resolve(registrationList[index]), index);
                }

                return array;
            }

            public object Activate(IResolver resolver, IReadOnlyList<IParameter> parameterList)
            {
                throw new NotImplementedException();
            }

            public object Activate(object[] parameterList)
            {
                var array = Array.CreateInstance(ElementType, parameterList.Length);

                for (var index = 0; index < parameterList.Length; index++)
                {
                    var parameter = parameterList[index];
                    var parameterType = parameter.GetType();

                    // TODO: Type.IsInstanceOfType(object)?
                    if (!ElementType.IsAssignableFrom(parameterType))
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
