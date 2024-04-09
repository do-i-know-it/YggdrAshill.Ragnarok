using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal static class TypeAnalysis
    {
        private static readonly ConcurrentDictionary<Type, InstantiationRequest> instantiationRequestCache = new();
        public static InstantiationRequest GetInstantiationRequest(Type type, Func<Type, InstantiationRequest> creation)
        {
            return instantiationRequestCache.GetOrAdd(type, creation);
        }

        private static readonly ConcurrentDictionary<Type, InjectionRequest> fieldInjectionRequestCache = new();
        public static InjectionRequest GetFieldInjectionRequest(Type type, Func<Type, InjectionRequest> creation)
        {
            return fieldInjectionRequestCache.GetOrAdd(type, creation);
        }

        private static readonly ConcurrentDictionary<Type, InjectionRequest> propertyInjectionRequestCache = new();
        public static InjectionRequest GetPropertyInjectionRequest(Type type, Func<Type, InjectionRequest> creation)
        {
            return propertyInjectionRequestCache.GetOrAdd(type, creation);
        }

        private static readonly ConcurrentDictionary<Type, InjectionRequest> methodInjectionRequestCache = new();
        public static InjectionRequest GetMethodInjectionRequest(Type type, Func<Type, InjectionRequest> creation)
        {
            return methodInjectionRequestCache.GetOrAdd(type, creation);
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

            if (instantiationRequestCache.TryGetValue(current, out var instantiationRequest))
            {
                foreach (var type in instantiationRequest.Dependency.DependentTypeList)
                {
                    if (CanResolve(resolver, type, out var description))
                    {
                        CheckCircularDependencyRecursively(description.ImplementedType, resolver, stack);
                    }
                }
            }

            if (methodInjectionRequestCache.TryGetValue(current, out var methodInjection))
            {
                foreach (var type in methodInjection.Dependency.DependentTypeList)
                {
                    if (CanResolve(resolver, type, out var description))
                    {
                        CheckCircularDependencyRecursively(description.ImplementedType, resolver, stack);
                    }
                }
            }

            if (fieldInjectionRequestCache.TryGetValue(current, out var fieldInjection))
            {
                foreach (var type in fieldInjection.Dependency.DependentTypeList)
                {
                    if (CanResolve(resolver, type, out var description))
                    {
                        CheckCircularDependencyRecursively(description.ImplementedType, resolver, stack);
                    }
                }
            }

            if (propertyInjectionRequestCache.TryGetValue(current, out var propertyInjection))
            {
                foreach (var type in propertyInjection.Dependency.DependentTypeList)
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
