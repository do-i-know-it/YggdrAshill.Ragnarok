using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok
{
    internal static class TypeAnalysis
    {
        public static Type OpenGenericTypeOf(Type closedGenericType)
        {
            return openGenericTypeCache.GetOrAdd(closedGenericType, createOpenGenericTypeFunctionCache);
        }
        private static readonly ConcurrentDictionary<Type, Type> openGenericTypeCache
            = new ConcurrentDictionary<Type, Type>();
        private static readonly Func<Type, Type> createOpenGenericTypeFunctionCache
            = CreateOpenGenericType;
        private static Type CreateOpenGenericType(Type closedGenericType)
        {
            return closedGenericType.GetGenericTypeDefinition();
        }

        public static Type[] GenericTypeParameterListOf(Type closedGenericType)
        {
            return genericTypeParameterListCache.GetOrAdd(closedGenericType, genericTypeParameterListFunctionCache);
        }
        private static readonly ConcurrentDictionary<Type, Type[]> genericTypeParameterListCache
            = new ConcurrentDictionary<Type, Type[]>();
        private static readonly Func<Type, Type[]> genericTypeParameterListFunctionCache
            = CreateGenericTypeParameterList;
        private static Type[] CreateGenericTypeParameterList(Type closedGenericType)
        {
            return closedGenericType.GetGenericArguments();
        }

        public static Type ArrayTypeOf(Type elementType)
        {
            return arrayTypeCache.GetOrAdd(elementType, createArrayTypeFunctionCache);
        }
        private static readonly ConcurrentDictionary<Type, Type> arrayTypeCache = new ConcurrentDictionary<Type, Type>();
        private static readonly Func<Type, Type> createArrayTypeFunctionCache = CreateArrayType;
        private static Type CreateArrayType(Type elementType)
        {
            return elementType.MakeArrayType();
        }

        public static Type EnumerableOf(Type elementType)
        {
            return enumerableTypeCache.GetOrAdd(elementType, createEnumerableFunctionCache);
        }
        private static readonly ConcurrentDictionary<Type, Type> enumerableTypeCache
            = new ConcurrentDictionary<Type, Type>();
        private static readonly Func<Type, Type> createEnumerableFunctionCache = CreateEnumerable;
        private static Type CreateEnumerable(Type elementType)
        {
            return typeof(IEnumerable<>).MakeGenericType(elementType);
        }

        public static Type ReadOnlyListOf(Type elementType)
        {
            return readOnlyListTypeCache.GetOrAdd(elementType, createReadOnlyListFunctionCache);
        }
        private static readonly ConcurrentDictionary<Type, Type> readOnlyListTypeCache
            = new ConcurrentDictionary<Type, Type>();
        private static readonly Func<Type, Type> createReadOnlyListFunctionCache = CreateReadOnlyList;
        private static Type CreateReadOnlyList(Type elementType)
        {
            return typeof(IReadOnlyList<>).MakeGenericType(elementType);
        }

        public static Type ReadOnlyCollectionOf(Type elementType)
        {
            return readOnlyCollectionTypeCache.GetOrAdd(elementType, createReadOnlyCollectionTypeFunctionCache);
        }

        private static readonly ConcurrentDictionary<Type, Type> readOnlyCollectionTypeCache
            = new ConcurrentDictionary<Type, Type>();
        private static readonly Func<Type, Type> createReadOnlyCollectionTypeFunctionCache
            = CreateReadOnlyCollectionType;
        private static Type CreateReadOnlyCollectionType(Type elementType)
        {
            return typeof(IReadOnlyCollection<>).MakeGenericType(elementType);
        }

        private static readonly ConcurrentDictionary<Type, IActivation> activationCache
            = new ConcurrentDictionary<Type, IActivation>();
        public static IActivation GetActivation(Type type, Func<Type, IActivation> creation)
        {
            return activationCache.GetOrAdd(type, creation);
        }

        private static readonly ConcurrentDictionary<Type, IInfusion> fieldInfusionCache
            = new ConcurrentDictionary<Type, IInfusion>();
        public static IInfusion GetFieldInfusion(Type type, Func<Type, IInfusion> creation)
        {
            return fieldInfusionCache.GetOrAdd(type, creation);
        }

        private static readonly ConcurrentDictionary<Type, IInfusion> propertyInfusionCache
            = new ConcurrentDictionary<Type, IInfusion>();
        public static IInfusion GetPropertyInfusion(Type type, Func<Type, IInfusion> creation)
        {
            return propertyInfusionCache.GetOrAdd(type, creation);
        }

        private static readonly ConcurrentDictionary<Type, IInfusion> methodInfusionCache
            = new ConcurrentDictionary<Type, IInfusion>();
        public static IInfusion GetMethodInfusion(Type type, Func<Type, IInfusion> creation)
        {
            return methodInfusionCache.GetOrAdd(type, creation);
        }

        [ThreadStatic]
        private static Stack<Type>? typeStack;
        private static Stack<Type> TypeStack
        {
            get
            {
                if (typeStack == null)
                {
                    typeStack = new Stack<Type>();
                }

                return typeStack;
            }
        }

        public static void Validate(IEnumerable<IDescription> descriptionList, IEngine engine)
        {
            foreach (var description in descriptionList)
            {
                TypeStack.Clear();
                CheckCircularDependencyRecursively(description.ImplementedType, engine, TypeStack);
            }
        }
        private static void CheckCircularDependencyRecursively(Type current, IEngine engine, Stack<Type> stack)
        {
            foreach (var stacked in stack)
            {
                if (current == stacked)
                {
                    throw new RagnarokCircularDependencyDetectedException(current, $"Circular dependency detected in {current}.");
                }
            }

            stack.Push(current);

            if (activationCache.TryGetValue(current, out var constructorInjection))
            {
                var dependentTypeList
                    = constructorInjection.ArgumentList.Select(argument => argument.Type).Distinct();
                foreach (var type in dependentTypeList)
                {
                    if (engine.Find(type, out var registration))
                    {
                        CheckCircularDependencyRecursively(registration.ImplementedType, engine, stack);
                    }
                }
            }

            if (methodInfusionCache.TryGetValue(current, out var methodInjection))
            {
                foreach (var type in methodInjection.ArgumentList.Select(argument => argument.Type).Distinct())
                {
                    if (engine.Find(type, out var registration))
                    {
                        CheckCircularDependencyRecursively(registration.ImplementedType, engine, stack);
                    }
                }
            }

            if (fieldInfusionCache.TryGetValue(current, out var fieldInjection))
            {
                foreach (var type in fieldInjection.ArgumentList.Select(argument => argument.Type).Distinct())
                {
                    if (engine.Find(type, out var registration))
                    {
                        CheckCircularDependencyRecursively(registration.ImplementedType, engine, stack);
                    }
                }
            }

            if (propertyInfusionCache.TryGetValue(current, out var propertyInjection))
            {
                foreach (var type in propertyInjection.ArgumentList.Select(argument => argument.Type).Distinct())
                {
                    if (engine.Find(type, out var registration))
                    {
                        CheckCircularDependencyRecursively(registration.ImplementedType, engine, stack);
                    }
                }
            }

            stack.Pop();
        }
    }
}
