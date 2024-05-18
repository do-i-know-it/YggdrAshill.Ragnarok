using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal readonly struct CircularDependencyDetection : IDisposable
    {
        private readonly TypeAnalysis analysis;
        private readonly IScopedResolver resolver;
        private readonly IEnumerable<IStatement> statementList;
        private readonly Stack<Type> dependencyStack;

        public CircularDependencyDetection(TypeAnalysis analysis, IScopedResolver resolver, IEnumerable<IStatement> statementList)
        {
            this.analysis = analysis;
            this.resolver = resolver;
            this.statementList = statementList;
            dependencyStack = new Stack<Type>();
        }

        public void Detect()
        {
            foreach (var statement in statementList)
            {
                dependencyStack.Clear();
                CheckCircularDependencyRecursively(statement.ImplementedType);
            }
        }

        private void CheckCircularDependencyRecursively(Type current)
        {
            foreach (var stacked in dependencyStack)
            {
                if (current == stacked)
                {
                    throw new RagnarokCircularDependencyException(current);
                }
            }

            dependencyStack.Push(current);

            if (analysis.instantiationRequestCache.TryGetValue(current, out var instantiationRequest))
            {
                foreach (var type in instantiationRequest.Dependency.DependentTypeList)
                {
                    if (CanResolve(resolver, type, out var description))
                    {
                        CheckCircularDependencyRecursively(description.ImplementedType);
                    }
                }
            }

            if (analysis.methodInjectionRequestCache.TryGetValue(current, out var methodInjection))
            {
                foreach (var type in methodInjection.Dependency.DependentTypeList)
                {
                    if (CanResolve(resolver, type, out var description))
                    {
                        CheckCircularDependencyRecursively(description.ImplementedType);
                    }
                }
            }

            if (analysis.fieldInjectionRequestCache.TryGetValue(current, out var fieldInjection))
            {
                foreach (var type in fieldInjection.Dependency.DependentTypeList)
                {
                    if (CanResolve(resolver, type, out var description))
                    {
                        CheckCircularDependencyRecursively(description.ImplementedType);
                    }
                }
            }

            if (analysis.propertyInjectionRequestCache.TryGetValue(current, out var propertyInjection))
            {
                foreach (var type in propertyInjection.Dependency.DependentTypeList)
                {
                    if (CanResolve(resolver, type, out var description))
                    {
                        CheckCircularDependencyRecursively(description.ImplementedType);
                    }
                }
            }

            dependencyStack.Pop();
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

        public void Dispose()
        {
            dependencyStack.Clear();
        }
    }
}
