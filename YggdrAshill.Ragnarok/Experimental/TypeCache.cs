using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal static class TypeCache
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
    }
}
