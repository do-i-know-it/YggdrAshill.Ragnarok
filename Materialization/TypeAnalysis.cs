using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok
{
    internal sealed class TypeAnalysis
    {
        private static readonly ConcurrentDictionary<Type, Type> openGenericTypeCache
            = new ConcurrentDictionary<Type, Type>();
        private static readonly Func<Type, Type> createOpenGenericType = CreateOpenGenericOf;
        private static Type CreateOpenGenericOf(Type closedGenericType)
        {
            return closedGenericType.GetGenericTypeDefinition();
        }
        public static Type OpenGenericTypeOf(Type closedGenericType)
            => openGenericTypeCache.GetOrAdd(closedGenericType, createOpenGenericType);

        private static readonly ConcurrentDictionary<Type, Type[]> genericTypeParameterListCache
            = new ConcurrentDictionary<Type, Type[]>();
        private static readonly Func<Type, Type[]> genericTypeParameterList = CreateGenericTypeParameterListOf;
        private static Type[] CreateGenericTypeParameterListOf(Type closedGenericType)
        {
            return closedGenericType.GetGenericArguments();
        }
        public static Type[] GenericTypeParameterListOf(Type closedGenericType)
            => genericTypeParameterListCache.GetOrAdd(closedGenericType, genericTypeParameterList);

        private static readonly ConcurrentDictionary<Type, Type> arrayTypeCache
            = new ConcurrentDictionary<Type, Type>();
        private static readonly Func<Type, Type> createArray = CreateArrayOf;
        private static Type CreateArrayOf(Type elementType)
        {
            return elementType.MakeArrayType();
        }
        public static Type ArrayTypeOf(Type elementType)
            => arrayTypeCache.GetOrAdd(elementType, createArray);

        private static readonly ConcurrentDictionary<Type, Type> enumerableTypeCache
            = new ConcurrentDictionary<Type, Type>();
        private static readonly Func<Type, Type> createEnumerable = CreateEnumerableOf;
        private static Type CreateEnumerableOf(Type elementType)
        {
            return typeof(IEnumerable<>).MakeGenericType(elementType);
        }
        public static Type EnumerableOf(Type elementType)
            => enumerableTypeCache.GetOrAdd(elementType, createEnumerable);

        private static readonly ConcurrentDictionary<Type, Type> readOnlyListTypeCache
            = new ConcurrentDictionary<Type, Type>();
        private static readonly Func<Type, Type> createReadOnlyList = CreateReadOnlyListOf;
        private static Type CreateReadOnlyListOf(Type elementType)
        {
            return typeof(IReadOnlyList<>).MakeGenericType(elementType);
        }
        public static Type ReadOnlyListOf(Type elementType)
            => readOnlyListTypeCache.GetOrAdd(elementType, createReadOnlyList);

        private static readonly ConcurrentDictionary<Type, Type> readOnlyCollectionTypeCache
            = new ConcurrentDictionary<Type, Type>();
        private static readonly Func<Type, Type> createReadOnlyCollection = CreateReadOnlyCollectionOf;
        private static Type CreateReadOnlyCollectionOf(Type elementType)
        {
            return typeof(IReadOnlyCollection<>).MakeGenericType(elementType);
        }
        public static Type ReadOnlyCollectionOf(Type elementType)
            => readOnlyCollectionTypeCache.GetOrAdd(elementType, createReadOnlyCollection);

        private readonly ConcurrentDictionary<Type, IActivation> localActivationCache;
        private readonly ConcurrentDictionary<Type, IInfusion> localFieldInfusionCache;
        private readonly ConcurrentDictionary<Type, IInfusion> localPropertyInfusionCache;
        private readonly ConcurrentDictionary<Type, IInfusion> localMethodInfusionCache;

        public TypeAnalysis()
        {
            localActivationCache = new ConcurrentDictionary<Type, IActivation>();
            localFieldInfusionCache = new ConcurrentDictionary<Type, IInfusion>();
            localPropertyInfusionCache = new ConcurrentDictionary<Type, IInfusion>();
            localMethodInfusionCache = new ConcurrentDictionary<Type, IInfusion>();
        }

        public TypeAnalysis(TypeAnalysis analysis)
        {
            localActivationCache = new ConcurrentDictionary<Type, IActivation>(analysis.localActivationCache);
            localFieldInfusionCache = new ConcurrentDictionary<Type, IInfusion>(analysis.localFieldInfusionCache);
            localPropertyInfusionCache = new ConcurrentDictionary<Type, IInfusion>(analysis.localPropertyInfusionCache);
            localMethodInfusionCache = new ConcurrentDictionary<Type, IInfusion>(analysis.localMethodInfusionCache);
        }

        public IActivation GetActivation(Type type, Func<Type, IActivation> creation)
        {
            return localActivationCache.GetOrAdd(type, creation);
        }

        public IInfusion GetFieldInfusion(Type type, Func<Type, IInfusion> creation)
        {
            return localFieldInfusionCache.GetOrAdd(type, creation);
        }

        public IInfusion GetPropertyInfusion(Type type, Func<Type, IInfusion> creation)
        {
            return localPropertyInfusionCache.GetOrAdd(type, creation);
        }

        public IInfusion GetMethodInfusion(Type type, Func<Type, IInfusion> creation)
        {
            return localMethodInfusionCache.GetOrAdd(type, creation);
        }

        [ThreadStatic]
        private static Stack<Type>? circularDependencyChecker;

        public void Validate(IEnumerable<IDescription> descriptionList, IEngine engine)
        {
            // ThreadStatic
            if (circularDependencyChecker == null)
            {
                circularDependencyChecker = new Stack<Type>();
            }

            foreach (var description in descriptionList)
            {
                circularDependencyChecker.Clear();
                CheckCircularDependencyRecursively(description.ImplementedType, engine, circularDependencyChecker);
            }
        }
        private void CheckCircularDependencyRecursively(Type current, IEngine engine, Stack<Type> stack)
        {
            foreach (var stacked in stack)
            {
                if (current == stacked)
                {
                    // TODO: throw original exception.
                    throw new Exception($"Circular dependency detected!");
                }
            }

            stack.Push(current);

            if (localActivationCache.TryGetValue(current, out var constructorInjection))
            {
                var dependentTypeList
                    = constructorInjection.ArgumentList.Select(argument => argument.Type).Distinct();
                foreach (var type in dependentTypeList)
                {
                    if (engine.Find(type, out var registration) && registration != null)
                    {
                        CheckCircularDependencyRecursively(registration.ImplementedType, engine, stack);
                    }

                    CheckCircularDependencyRecursively(type, engine, stack);
                }
            }

            if (localMethodInfusionCache.TryGetValue(current, out var methodInjection))
            {
                foreach (var type in methodInjection.ArgumentList.Select(argument => argument.Type).Distinct())
                {
                    if (engine.Find(type, out var registration) && registration != null)
                    {
                        CheckCircularDependencyRecursively(registration.ImplementedType, engine, stack);
                    }
                }
            }

            if (localFieldInfusionCache.TryGetValue(current, out var fieldInjection))
            {
                foreach (var type in fieldInjection.ArgumentList.Select(argument => argument.Type).Distinct())
                {
                    if (engine.Find(type, out var registration) && registration != null)
                    {
                        CheckCircularDependencyRecursively(registration.ImplementedType, engine, stack);
                    }
                }
            }

            if (localPropertyInfusionCache.TryGetValue(current, out var propertyInjection))
            {
                foreach (var type in propertyInjection.ArgumentList.Select(argument => argument.Type).Distinct())
                {
                    if (engine.Find(type, out var registration) && registration != null)
                    {
                        CheckCircularDependencyRecursively(registration.ImplementedType, engine, stack);
                    }
                }
            }

            stack.Pop();
        }
    }
}
