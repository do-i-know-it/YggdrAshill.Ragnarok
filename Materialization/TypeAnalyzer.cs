using YggdrAshill.Ragnarok.Hierarchization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok.Materialization
{
    public sealed class TypeAnalyzer
    {
        private static readonly TypeAnalyzer instance = new TypeAnalyzer();

        private TypeAnalyzer()
        {

        }

        private readonly ConcurrentDictionary<Type, IActivation> activationCache
            = new ConcurrentDictionary<Type, IActivation>();
        public static IActivation GetActivation(Type type, Func<Type, IActivation> creation)
        {
            return instance.activationCache.GetOrAdd(type, creation);
        }

        private readonly ConcurrentDictionary<Type, IInfusion> fieldCache
            = new ConcurrentDictionary<Type, IInfusion>();
        public static IInfusion GetFieldInfusion(Type type, Func<Type, IInfusion> creation)
        {
            return instance.fieldCache.GetOrAdd(type, creation);
        }

        private readonly ConcurrentDictionary<Type, IInfusion> propertyCache
            = new ConcurrentDictionary<Type, IInfusion>();
        public static IInfusion GetPropertyInfusion(Type type, Func<Type, IInfusion> creation)
        {
            return instance.propertyCache.GetOrAdd(type, creation);
        }

        private readonly ConcurrentDictionary<Type, IInfusion> methodCache
            = new ConcurrentDictionary<Type, IInfusion>();
        public static IInfusion GetMethodInfusion(Type type, Func<Type, IInfusion> creation)
        {
            return instance.methodCache.GetOrAdd(type, creation);
        }

        [ThreadStatic]
        private static Stack<Type>? circularDependencyChecker;

        public static void Validate(IEnumerable<IRegistration> registrationList, IRegistry registry)
        {
            // ThreadStatic
            if (circularDependencyChecker == null)
            {
                circularDependencyChecker = new Stack<Type>();
            }

            foreach (var registration in registrationList)
            {
                circularDependencyChecker.Clear();
                instance.CheckCircularDependencyRecursively(registration.ImplementedType, registry, circularDependencyChecker);
            }
        }
        private void CheckCircularDependencyRecursively(Type current, IRegistry registry, Stack<Type> stack)
        {
            foreach (var stacked in stack)
            {
                if (current == stacked)
                {
                    throw new Exception($"Circular dependency detected!");
                }
            }

            stack.Push(current);

            if (activationCache.TryGetValue(current, out var constructorInjection))
            {
                var dependentTypeList
                    = constructorInjection.ArgumentList.Select(argument => argument.Type).Distinct();
                foreach (var type in dependentTypeList)
                {
                    if (registry.TryGet(type, out var registration) && registration != null)
                    {
                        CheckCircularDependencyRecursively(registration.ImplementedType, registry, stack);
                    }

                    CheckCircularDependencyRecursively(type, registry, stack);
                }
            }

            if (methodCache.TryGetValue(current, out var methodInjection))
            {
                foreach (var type in methodInjection.ArgumentList.Select(argument => argument.Type).Distinct())
                {
                    if (registry.TryGet(type, out var registration) && registration != null)
                    {
                        CheckCircularDependencyRecursively(registration.ImplementedType, registry, stack);
                    }
                }
            }

            if (fieldCache.TryGetValue(current, out var fieldInjection))
            {
                foreach (var type in fieldInjection.ArgumentList.Select(argument => argument.Type).Distinct())
                {
                    if (registry.TryGet(type, out var registration) && registration != null)
                    {
                        CheckCircularDependencyRecursively(registration.ImplementedType, registry, stack);
                    }
                }
            }

            if (propertyCache.TryGetValue(current, out var propertyInjection))
            {
                foreach (var type in propertyInjection.ArgumentList.Select(argument => argument.Type).Distinct())
                {
                    if (registry.TryGet(type, out var registration) && registration != null)
                    {
                        CheckCircularDependencyRecursively(registration.ImplementedType, registry, stack);
                    }
                }
            }

            stack.Pop();
        }
    }
}
