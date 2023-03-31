using YggdrAshill.Ragnarok.Hierarchization;
using YggdrAshill.Ragnarok.Motorization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

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

        private readonly ConcurrentDictionary<Type, IInfusion> fieldInjectionCache
            = new ConcurrentDictionary<Type, IInfusion>();
        public static IInfusion GetFieldInjection(Type type, Func<Type, IInfusion> creation)
        {
            return instance.fieldInjectionCache.GetOrAdd(type, creation);
        }

        private readonly ConcurrentDictionary<Type, IInfusion> propertyInjectionCache
            = new ConcurrentDictionary<Type, IInfusion>();
        public static IInfusion GetPropertyInjection(Type type, Func<Type, IInfusion> creation)
        {
            return instance.propertyInjectionCache.GetOrAdd(type, creation);
        }

        private readonly ConcurrentDictionary<Type, IInfusion> methodInjectionCache
            = new ConcurrentDictionary<Type, IInfusion>();
        public static IInfusion GetMethodInjection(Type type, Func<Type, IInfusion> creation)
        {
            return instance.methodInjectionCache.GetOrAdd(type, creation);
        }

        [ThreadStatic]
        private static Stack<Type>? circularDependencyChecker;

        public static void Validate(IEnumerable<IRegistration> registrationList, IEngine engine)
        {
            // ThreadStatic
            if (circularDependencyChecker == null)
            {
                circularDependencyChecker = new Stack<Type>();
            }

            foreach (var registration in registrationList)
            {
                circularDependencyChecker.Clear();
                instance.CheckCircularDependencyRecursively(registration.ImplementedType, engine, circularDependencyChecker);
            }
        }
        private void CheckCircularDependencyRecursively(Type current, IEngine engine, Stack<Type> stack)
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
                foreach (var type in constructorInjection.DependentTypeList)
                {
                    if (engine.Find(type, out var registration))
                    {
                        CheckCircularDependencyRecursively(registration.ImplementedType, engine, stack);
                    }

                    CheckCircularDependencyRecursively(type, engine, stack);
                }
            }

            if (methodInjectionCache.TryGetValue(current, out var methodInjection))
            {
                foreach (var type in methodInjection.DependentTypeList)
                {
                    if (engine.Find(type, out var registration))
                    {
                        CheckCircularDependencyRecursively(registration.ImplementedType, engine, stack);
                    }
                }
            }

            if (fieldInjectionCache.TryGetValue(current, out var fieldInjection))
            {
                foreach (var type in fieldInjection.DependentTypeList)
                {
                    if (engine.Find(type, out var registration))
                    {
                        CheckCircularDependencyRecursively(registration.ImplementedType, engine, stack);
                    }
                }
            }

            if (propertyInjectionCache.TryGetValue(current, out var propertyInjection))
            {
                foreach (var type in propertyInjection.DependentTypeList)
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
