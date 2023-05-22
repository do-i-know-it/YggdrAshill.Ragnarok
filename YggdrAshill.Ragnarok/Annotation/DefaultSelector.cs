using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace YggdrAshill.Ragnarok
{
    public sealed class DefaultSelector :
        ISelector
    {
        public static DefaultSelector Instance { get; } = new DefaultSelector();

        private DefaultSelector()
        {
            createServiceBundleType = CreateServiceBundleTypeOf;
        }

        private readonly ConcurrentDictionary<Type, Type> serviceBundleTypeCache
            = new ConcurrentDictionary<Type, Type>();
        private readonly Func<Type, Type> createServiceBundleType;
        private Type CreateServiceBundleTypeOf(Type elementType)
        {
            return typeof(ServiceBundle<>).MakeGenericType(elementType);
        }

        public Type GetServiceBundleType(Type elementType)
        {
            return serviceBundleTypeCache.GetOrAdd(elementType, createServiceBundleType);
        }

        public ConstructorInjection CreateConstructorInjection(Type type)
        {
            // TODO: check whether type is for concrete class.

            const BindingFlags BindingFlags
                = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            var injectedConstructor = default(ConstructorInfo);
            var constructorHavingMaxParameterCount = default(ConstructorInfo);

            foreach (var constructorInfo in type.GetTypeInfo().GetConstructors(BindingFlags))
            {
                if (constructorInfo.IsDefined(typeof(InjectAttribute), false))
                {
                    if (injectedConstructor != null)
                    {
                        throw new Exception($"Type found multiple [Inject] marked constructors, type: {type.Name}");
                    }

                    injectedConstructor = constructorInfo;

                    continue;
                }

                if (constructorHavingMaxParameterCount == null)
                {
                    constructorHavingMaxParameterCount = constructorInfo;
                    continue;
                }

                if (constructorHavingMaxParameterCount.GetParameters().Length < constructorInfo.GetParameters().Length)
                {
                    constructorHavingMaxParameterCount = constructorInfo;
                }
            }

            if (injectedConstructor != null)
            {
                return new ConstructorInjection(injectedConstructor);
            }

            if (constructorHavingMaxParameterCount != null)
            {
                return new ConstructorInjection(constructorHavingMaxParameterCount);
            }

            throw new Exception($"Type does not found injectable constructor, type: {type.Name}");
        }

        public FieldInjection CreateFieldInjection(Type type)
        {
            // TODO: check whether type is for concrete class.

            var buffer = default(List<FieldInfo>);
            foreach (var fieldInfo in type.GetRuntimeFields())
            {
                if (!fieldInfo.IsDefined(typeof(InjectFieldAttribute), true))
                {
                    continue;
                }

                if (buffer == null)
                {
                    // TODO: object pooling.
                    buffer = new List<FieldInfo>();
                }

                buffer.Add(fieldInfo);
            }

            if (buffer == null)
            {
                throw new Exception($"Type does not found injectable constructor, type: {type.Name}");
            }

            return new FieldInjection(type, buffer.ToArray());
        }

        public PropertyInjection CreatePropertyInjection(Type type)
        {
            // TODO: check whether type is for concrete class.

            var buffer = default(List<PropertyInfo>);
            foreach (var propertyInfo in type.GetRuntimeProperties())
            {
                if (!propertyInfo.IsDefined(typeof(InjectPropertyAttribute), true))
                {
                    continue;
                }

                if (propertyInfo.SetMethod == null)
                {
                    continue;
                }

                if (buffer == null)
                {
                    buffer = new List<PropertyInfo>();
                }

                buffer.Add(propertyInfo);
            }

            if (buffer == null)
            {
                throw new Exception($"Type does not found injectable constructor, type: {type.Name}");
            }

            return new PropertyInjection(type, buffer.ToArray());
        }

        public MethodInjection CreateMethodInjection(Type type)
        {
            var injectedMethod = default(MethodInfo);

            foreach (var methodInfo in type.GetRuntimeMethods())
            {
                if (!methodInfo.IsDefined(typeof(InjectMethodAttribute), true))
                {
                    continue;
                }

                if (injectedMethod != null)
                {
                    throw new Exception($"Type found multiple [Inject] marked constructors, type: {type.Name}");
                }

                injectedMethod = methodInfo;
            }

            if (injectedMethod == null)
            {
                throw new Exception($"Type does not found injectable method, type: {type.Name}");
            }

            return new MethodInjection(type, injectedMethod);
        }
    }
}
