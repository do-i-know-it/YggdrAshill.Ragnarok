using YggdrAshill.Ragnarok.Fabrication;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation of <see cref="ISelector"/> finding annotation.
    /// </summary>
    public sealed class AnnotationSelector : ISelector
    {
        /// <summary>
        /// Singleton of <see cref="AnnotationSelector"/>.
        /// </summary>
        public static AnnotationSelector Instance { get; } = new();

        private AnnotationSelector()
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
        public Type GetServiceBundleType(Type elementType)
        {
            return serviceBundleTypeCache.GetOrAdd(elementType, createServiceBundleType);
        }

        /// <inheritdoc/>
        public ConstructorInjection CreateConstructorInjection(Type type)
        {
            if (!ValidateType.IsInstantiatable(type))
            {
                throw new RagnarokNotInstantiatableException(type);
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
                return new ConstructorInjection(type, injectedConstructor);
            }

            if (constructorHavingMaxParameterCount != null)
            {
                return new ConstructorInjection(type, constructorHavingMaxParameterCount);
            }

            throw new RagnarokNotAnnotatedException(type, $"Injectable constructor of {type} not found.");
        }

        /// <inheritdoc/>
        public FieldInjection CreateFieldInjection(Type type)
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

            return new FieldInjection(type, buffer.ToArray());
        }

        /// <inheritdoc/>
        public PropertyInjection CreatePropertyInjection(Type type)
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

            return new PropertyInjection(type, buffer.ToArray());
        }

        /// <inheritdoc/>
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
                    throw new RagnarokAlreadyAnnotatedException(type, $"Multiple injectable methods of {type} found.");
                }

                injectedMethod = methodInfo;
            }

            if (injectedMethod == null)
            {
                throw new RagnarokNotAnnotatedException(type, $"Injectable method of {type} not found.");
            }

            return new MethodInjection(type, injectedMethod);
        }
    }
}
