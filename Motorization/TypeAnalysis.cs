using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class TypeAnalysis
    {
        private static readonly Func<Type, Type> createOpenGenericType;
        private static readonly Func<Type, Type[]> createGenericTypeParameterList;
        private static readonly Func<Type, Type> createArrayElementType;
        private static readonly Func<Type, Type> createArrayType;
        private static readonly Func<Type, Type> createReadOnlyListFunctionCache;
        private static readonly Func<Type, Type> createReadOnlyCollectionType;
        private static readonly Func<Type, Type> createEnumerable;

        static TypeAnalysis()
        {
            createOpenGenericType = closedGenericType => closedGenericType.GetGenericTypeDefinition();
            createOpenGenericType = closedGenericType => closedGenericType.GetGenericTypeDefinition();
            createGenericTypeParameterList = closedGenericType => closedGenericType.GetGenericArguments();
            createArrayType = elementType => elementType.MakeArrayType();
            createArrayElementType = arrayType => arrayType.GetElementType()!;
            createReadOnlyListFunctionCache = elementType => typeof(IReadOnlyList<>).MakeGenericType(elementType);
            createReadOnlyCollectionType = elementType => typeof(IReadOnlyCollection<>).MakeGenericType(elementType);
            createEnumerable = elementType => typeof(IEnumerable<>).MakeGenericType(elementType);
        }

        private readonly ConcurrentDictionary<Type, Type> openGenericTypeCache = new();
        public bool IsGeneric(Type closedGenericType, out Type openedGenericType)
        {
            if (closedGenericType.IsConstructedGenericType)
            {
                openedGenericType = openGenericTypeCache.GetOrAdd(closedGenericType, createOpenGenericType);
                return true;
            }

            openedGenericType = default!;
            return false;
        }

        private readonly ConcurrentDictionary<Type, Type[]> genericTypeParameterListCache = new();
        public Type[] GetGenericTypeParameterList(Type closedGenericType)
        {
            return genericTypeParameterListCache.GetOrAdd(closedGenericType, createGenericTypeParameterList);
        }

        private readonly ConcurrentDictionary<Type, Type> arrayElementTypeCache = new();
        public bool IsArray(Type type, out Type elementType)
        {
            if (type.IsArray && type.GetArrayRank() == 1)
            {
                elementType = arrayElementTypeCache.GetOrAdd(type, createArrayElementType);
                return true;
            }

            elementType = default!;
            return false;
        }

        private readonly ConcurrentDictionary<Type, Type> arrayTypeCache = new();
        public Type GetArrayType(Type elementType)
        {
            return arrayTypeCache.GetOrAdd(elementType, createArrayType);
        }

        private readonly ConcurrentDictionary<Type, Type> readOnlyListTypeCache = new();
        public Type GetReadOnlyListType(Type elementType)
        {
            return readOnlyListTypeCache.GetOrAdd(elementType, createReadOnlyListFunctionCache);
        }

        private readonly ConcurrentDictionary<Type, Type> readOnlyCollectionTypeCache = new();
        public Type GetReadOnlyCollectionType(Type elementType)
        {
            return readOnlyCollectionTypeCache.GetOrAdd(elementType, createReadOnlyCollectionType);
        }

        private readonly ConcurrentDictionary<Type, Type> enumerableTypeCache = new();
        public Type GetEnumerableType(Type elementType)
        {
            return enumerableTypeCache.GetOrAdd(elementType, createEnumerable);
        }

        internal readonly ConcurrentDictionary<Type, InstantiationRequest> instantiationRequestCache = new();
        public InstantiationRequest GetInstantiationRequest(Type type, Func<Type, InstantiationRequest> creation)
        {
            return instantiationRequestCache.GetOrAdd(type, creation);
        }

        internal readonly ConcurrentDictionary<Type, InjectionRequest> fieldInjectionRequestCache = new();
        public InjectionRequest GetFieldInjectionRequest(Type type, Func<Type, InjectionRequest> creation)
        {
            return fieldInjectionRequestCache.GetOrAdd(type, creation);
        }

        internal readonly ConcurrentDictionary<Type, InjectionRequest> propertyInjectionRequestCache = new();
        public InjectionRequest GetPropertyInjectionRequest(Type type, Func<Type, InjectionRequest> creation)
        {
            return propertyInjectionRequestCache.GetOrAdd(type, creation);
        }

        internal readonly ConcurrentDictionary<Type, InjectionRequest> methodInjectionRequestCache = new();
        public InjectionRequest GetMethodInjectionRequest(Type type, Func<Type, InjectionRequest> creation)
        {
            return methodInjectionRequestCache.GetOrAdd(type, creation);
        }

        public void Validate(IEnumerable<IStatement> statementList, IScopedResolver resolver)
        {
            using var detection = new CircularDependencyDetection(this, resolver, statementList);
            detection.Detect();
        }
    }
}
