using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace YggdrAshill.Ragnarok
{
    public sealed class AnnotationSelector :
        ISelector
    {
        public static AnnotationSelector Instance { get; } = new AnnotationSelector();

        private AnnotationSelector()
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
            if (!ValidateType.IsInstantiatable(type))
            {
                // TODO: throw original exception.
                throw new Exception($"{type} is not instantiatable.");
            }

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
                        // TODO: throw original exception.
                        throw new Exception($"Type found multiple constructors marked [Inject], type: {type.Name}");
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

            // TODO: throw original exception.
            throw new Exception($"Type does not found injectable constructor, type: {type.Name}");
        }

        public FieldInjection CreateFieldInjection(Type type)
        {
            // TODO: concrete class?

            var buffer = default(List<FieldInfo>);
            foreach (var fieldInfo in type.GetRuntimeFields())
            {
                if (!fieldInfo.IsDefined(typeof(InjectFieldAttribute), true))
                {
                    continue;
                }

                if (fieldInfo.IsInitOnly)
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
                // TODO: throw original exception.
                throw new Exception($"Type does not found injectable field, type: {type.Name}");
            }

            return new FieldInjection(type, buffer.ToArray());
        }

        public PropertyInjection CreatePropertyInjection(Type type)
        {
            // TODO: concrete class?

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
                    // TODO: object pooling.
                    buffer = new List<PropertyInfo>();
                }

                buffer.Add(propertyInfo);
            }

            if (buffer == null)
            {
                // TODO: throw original exception.
                throw new Exception($"Type does not found injectable property, type: {type.Name}");
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
                    // TODO: throw original exception.
                    throw new Exception($"Type found multiple methods marked [InjectMethod], type: {type.Name}");
                }

                injectedMethod = methodInfo;
            }

            if (injectedMethod == null)
            {
                // TODO: throw original exception.
                throw new Exception($"Type does not found injectable method, type: {type.Name}");
            }

            return new MethodInjection(type, injectedMethod);
        }
    }
}
