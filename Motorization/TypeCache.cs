using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal static class TypeCache
    {
        public static Type OpenGenericTypeOf(Type closedGenericType)
        {
            return openGenericTypeCache.GetOrAdd(closedGenericType, createOpenGenericType);
        }
        private static readonly ConcurrentDictionary<Type, Type> openGenericTypeCache = new();
        private static readonly Func<Type, Type> createOpenGenericType = CreateOpenGenericType;
        private static Type CreateOpenGenericType(Type closedGenericType)
        {
            return closedGenericType.GetGenericTypeDefinition();
        }

        public static Type[] GenericTypeParameterListOf(Type closedGenericType)
        {
            return genericTypeParameterListCache.GetOrAdd(closedGenericType, createGenericTypeParameterList);
        }
        private static readonly ConcurrentDictionary<Type, Type[]> genericTypeParameterListCache = new();
        private static readonly Func<Type, Type[]> createGenericTypeParameterList = CreateGenericTypeParameterList;
        private static Type[] CreateGenericTypeParameterList(Type closedGenericType)
        {
            return closedGenericType.GetGenericArguments();
        }

        public static Type ArrayTypeOf(Type elementType)
        {
            return arrayTypeCache.GetOrAdd(elementType, createArrayType);
        }
        private static readonly ConcurrentDictionary<Type, Type> arrayTypeCache = new();
        private static readonly Func<Type, Type> createArrayType = CreateArrayType;
        private static Type CreateArrayType(Type elementType)
        {
            return elementType.MakeArrayType();
        }

        public static Type OpenGenericEnumerable { get; } = typeof(IEnumerable<>);
        public static Type EnumerableOf(Type elementType)
        {
            return enumerableTypeCache.GetOrAdd(elementType, createEnumerable);
        }
        private static readonly ConcurrentDictionary<Type, Type> enumerableTypeCache = new();
        private static readonly Func<Type, Type> createEnumerable = CreateEnumerable;
        private static Type CreateEnumerable(Type elementType)
        {
            return OpenGenericEnumerable.MakeGenericType(elementType);
        }

        public static Type OpenGenericReadOnlyCollection { get; } = typeof(IReadOnlyCollection<>);
        public static Type ReadOnlyCollectionOf(Type elementType)
        {
            return readOnlyCollectionTypeCache.GetOrAdd(elementType, createReadOnlyCollectionType);
        }
        private static readonly ConcurrentDictionary<Type, Type> readOnlyCollectionTypeCache = new();
        private static readonly Func<Type, Type> createReadOnlyCollectionType = CreateReadOnlyCollectionType;
        private static Type CreateReadOnlyCollectionType(Type elementType)
        {
            return OpenGenericReadOnlyCollection.MakeGenericType(elementType);
        }

        public static Type OpenGenericReadOnlyList { get; } = typeof(IReadOnlyList<>);
        public static Type ReadOnlyListOf(Type elementType)
        {
            return readOnlyListTypeCache.GetOrAdd(elementType, createReadOnlyListFunctionCache);
        }
        private static readonly ConcurrentDictionary<Type, Type> readOnlyListTypeCache = new();
        private static readonly Func<Type, Type> createReadOnlyListFunctionCache = CreateReadOnlyList;
        private static Type CreateReadOnlyList(Type elementType)
        {
            return OpenGenericReadOnlyList.MakeGenericType(elementType);
        }

        public static Type OpenGenericServiceBundle { get; } = typeof(IServiceBundle<>);
    }
}
