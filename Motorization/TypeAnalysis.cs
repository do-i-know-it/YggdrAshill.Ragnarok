using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok
{
    internal static class TypeAnalysis
    {
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

        public static void Validate(IEnumerable<IStatement> statementList, IScopedResolver resolver)
        {
            foreach (var statement in statementList)
            {
                TypeStack.Clear();
                CheckCircularDependencyRecursively(statement.ImplementedType, resolver, TypeStack);
            }
        }
        private static void CheckCircularDependencyRecursively(Type current, IScopedResolver resolver, Stack<Type> stack)
        {
            foreach (var stacked in stack)
            {
                if (current == stacked)
                {
                    throw new RagnarokCircularDependencyException(current);
                }
            }

            stack.Push(current);

            if (activationCache.TryGetValue(current, out var constructorInjection))
            {
                var dependentTypeList
                    = constructorInjection.ArgumentList.Select(argument => argument.Type).Distinct();
                foreach (var type in dependentTypeList)
                {
                    if (CanResolve(resolver, type, out var description))
                    {
                        CheckCircularDependencyRecursively(description.ImplementedType, resolver, stack);
                    }
                }
            }

            if (methodInfusionCache.TryGetValue(current, out var methodInjection))
            {
                foreach (var type in methodInjection.ArgumentList.Select(argument => argument.Type).Distinct())
                {
                    if (CanResolve(resolver, type, out var description))
                    {
                        CheckCircularDependencyRecursively(description.ImplementedType, resolver, stack);
                    }
                }
            }

            if (fieldInfusionCache.TryGetValue(current, out var fieldInjection))
            {
                foreach (var type in fieldInjection.ArgumentList.Select(argument => argument.Type).Distinct())
                {
                    if (CanResolve(resolver, type, out var description))
                    {
                        CheckCircularDependencyRecursively(description.ImplementedType, resolver, stack);
                    }
                }
            }

            if (propertyInfusionCache.TryGetValue(current, out var propertyInjection))
            {
                foreach (var type in propertyInjection.ArgumentList.Select(argument => argument.Type).Distinct())
                {
                    if (CanResolve(resolver, type, out var description))
                    {
                        CheckCircularDependencyRecursively(description.ImplementedType, resolver, stack);
                    }
                }
            }

            stack.Pop();
        }

        private static bool CanResolve(IScopedResolver resolver, Type type, out IDescription description)
        {
            while (true)
            {
                if (resolver.CanResolve(type, out description))
                {
                    return true;
                }

                if (!resolver.CanEscalate(out resolver))
                {
                    return false;
                }
            }
        }
    }
}
