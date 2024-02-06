using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation of <see cref="IDecision"/> with annotation.
    /// </summary>
    public sealed class AnnotateToDecide : IDecision
    {
        /// <summary>
        /// Singleton of <see cref="AnnotateToDecide"/>.
        /// </summary>
        public static AnnotateToDecide Instance { get; } = new();

        private AnnotateToDecide()
        {
            createServiceBundleType = CreateServiceBundleTypeOf;
        }

        private readonly ConcurrentDictionary<Type, Type> serviceBundleTypeCache = new();
        private readonly Func<Type, Type> createServiceBundleType;
        private static Type CreateServiceBundleTypeOf(Type elementType)
        {
            return typeof(ServiceBundle<>).MakeGenericType(elementType);
        }

        /// <inheritdoc/>
        public ConstructorInjectionRequest RequestDependencyInjection(Type type)
        {
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
                        throw new RagnarokAlreadyAnnotatedException(type, $"Multiple injectable constructors of {type} found.");
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
                return new ConstructorInjectionRequest(type, injectedConstructor);
            }

            if (constructorHavingMaxParameterCount != null)
            {
                return new ConstructorInjectionRequest(type, constructorHavingMaxParameterCount);
            }

            throw new RagnarokNotAnnotatedException(type, $"Injectable constructor of {type} not found.");
        }

        /// <inheritdoc/>
        public FieldInjectionRequest RequestFieldInjection(Type type)
        {
            // TODO: concrete class?

            var buffer = new List<FieldInfo>();
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

                buffer.Add(fieldInfo);
            }

            return new FieldInjectionRequest(type, buffer.ToArray());
        }

        /// <inheritdoc/>
        public PropertyInjectionRequest RequestPropertyInjection(Type type)
        {
            // TODO: concrete class?

            var buffer = new List<PropertyInfo>();
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

                buffer.Add(propertyInfo);
            }

            return new PropertyInjectionRequest(type, buffer.ToArray());
        }

        /// <inheritdoc/>
        public MethodInjectionRequest RequestMethodInjection(Type type)
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
                    throw new RagnarokAlreadyAnnotatedException(type, $"Multiple injectable methods of {type} found.");
                }

                injectedMethod = methodInfo;
            }

            if (injectedMethod == null)
            {
                throw new RagnarokNotAnnotatedException(type, $"Injectable method of {type} not found.");
            }

            return new MethodInjectionRequest(type, injectedMethod);
        }

        /// <inheritdoc/>
        public ConstructorInjectionRequest RequestServiceBundleInjection(Type elementType)
        {
            var targetType = serviceBundleTypeCache.GetOrAdd(elementType, createServiceBundleType);

            return RequestDependencyInjection(targetType);
        }
    }
}
