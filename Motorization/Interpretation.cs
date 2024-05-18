using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace YggdrAshill.Ragnarok
{
    internal sealed class Interpretation : IInterpretation
    {
        private readonly IDependencyEnumeration enumeration;
        private readonly IDependencySelection selection;
        private readonly IDependencyOperation operation;

        private readonly Func<Type, InstantiationRequest> createActivation;
        private readonly Func<Type, InstantiationRequest> createServiceBundleActivation;
        private readonly Func<Type, InjectionRequest> createFieldInfusion;
        private readonly Func<Type, InjectionRequest> createPropertyInfusion;
        private readonly Func<Type, InjectionRequest> createMethodInfusion;

        public Interpretation(IDependencyEnumeration enumeration, IDependencySelection selection, IDependencyOperation operation)
        {
            this.enumeration = enumeration;
            this.selection = selection;
            this.operation = operation;

            createActivation = CreateActivation;
            createServiceBundleActivation = CreateServiceBundleActivation;
            createFieldInfusion = CreateFieldInfusion;
            createPropertyInfusion = CreatePropertyInfusion;
            createMethodInfusion = CreateMethodInfusion;
        }

        private readonly TypeAnalysis analysis = new();

        public bool CanResolveAsArray(Type type, out Type elementType)
        {
            if (analysis.IsArray(type, out elementType))
            {
                return true;
            }
            if (analysis.IsGeneric(type, out var openedGenericType))
            {
                if (openedGenericType == typeof(IReadOnlyList<>) ||
                    openedGenericType == typeof(IReadOnlyCollection<>) ||
                    openedGenericType == typeof(IEnumerable<>))
                {
                    elementType = analysis.GetGenericTypeParameterList(type)[0];
                    return true;
                }
            }

            elementType = default!;
            return false;
        }

        public IEnumerable<Type> GetAssignedTypeList(Type elementType)
        {
            return new[]
            {
                analysis.GetArrayType(elementType),
                analysis.GetReadOnlyListType(elementType),
                analysis.GetReadOnlyCollectionType(elementType),
                analysis.GetEnumerableType(elementType),
            };
        }

        public CollectionDescription CreateCollectionDescription(Type elementType, params IDescription[] descriptionList)
        {
            var implementedType = analysis.GetArrayType(elementType);
            var request = GetInstantiationRequest(implementedType);
            return new CollectionDescription(implementedType, request.Activation, descriptionList);
        }

        public bool IsServiceBundle(Type type, out Type elementType)
        {
            if (!analysis.IsGeneric(type, out var openedGenericType) || openedGenericType != typeof(ServiceBundle<>))
            {
                elementType = default!;
                return false;
            }

            elementType = analysis.GetGenericTypeParameterList(type)[0];
            return true;
        }

        public Type GetReadOnlyList(Type elementType)
        {
            return analysis.GetReadOnlyListType(elementType);
        }

        public ServiceBundleDescription CreateServiceBundleDescription(Type type, CollectionDescription collection)
        {
            var activation = analysis.GetInstantiationRequest(type, createServiceBundleActivation).Activation;
            return new ServiceBundleDescription(type, activation, collection);
        }

        private InstantiationRequest CreateServiceBundleActivation(Type type)
        {
            var constructor = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public)[0];
            var request = new ConstructorInjectionRequest(type, constructor);
            var activation = operation.CreateActivation(request);
            var argumentList = request.ParameterList.Select(info => new Argument(info.Name, info.ParameterType)).ToArray();
            var dependency = new WithDependency(argumentList);
            return new InstantiationRequest(dependency, activation);
        }

        public void Validate(IEnumerable<IStatement> statementList, IScopedResolver resolver)
        {
            analysis.Validate(statementList, resolver);
        }

        public InstantiationRequest GetInstantiationRequest(Type type)
        {
            return analysis.GetInstantiationRequest(type, createActivation);
        }
        private InstantiationRequest CreateActivation(Type type)
        {
            if (analysis.IsArray(type, out var elementType))
            {
                var activation = operation.CreateCollectionActivation(elementType);
                return new InstantiationRequest(WithoutDependency.Instance, activation);
            }
            var constructorList = enumeration.GetConstructorList(type).Where(selection.IsValid).ToArray();
            if (constructorList.Length == 0)
            {
                var activation = operation.CreateActivation(type);
                return new InstantiationRequest(WithoutDependency.Instance, activation);
            }
            if (constructorList.Length == 1)
            {
                var request = new ConstructorInjectionRequest(type, constructorList[0]);
                var activation = operation.CreateActivation(request);
                if (request.ParameterList.Length == 0)
                {
                    return new InstantiationRequest(WithoutDependency.Instance, activation);
                }

                var argumentList = request.ParameterList.Select(info => new Argument(info.Name, info.ParameterType)).ToArray();
                var dependency = new WithDependency(argumentList);
                return new InstantiationRequest(dependency, activation);
            }

            throw new RagnarokAlreadySelectedException(type, $"Multiple injectable constructors in {type}.");
        }

        public InjectionRequest GetFieldInjectionRequest(Type type)
        {
            return analysis.GetFieldInjectionRequest(type, createFieldInfusion);
        }
        private InjectionRequest CreateFieldInfusion(Type type)
        {
            var fieldList = enumeration.GetFieldList(type).Where(selection.IsValid).ToArray();
            if (fieldList.Length == 0)
            {
                return new InjectionRequest(WithoutDependency.Instance, InfuseNothing.Instance);
            }

            var request = new FieldInjectionRequest(type, fieldList);
            var infusion = operation.CreateFieldInfusion(request);
            var argumentList = fieldList.Select(info => new Argument(info.Name, info.FieldType)).ToArray();
            var dependency = new WithDependency(argumentList);
            return new InjectionRequest(dependency, infusion);
        }

        public InjectionRequest GetPropertyInjectionRequest(Type type)
        {
            return analysis.GetPropertyInjectionRequest(type, createPropertyInfusion);
        }
        private InjectionRequest CreatePropertyInfusion(Type type)
        {
            var propertyList = enumeration.GetPropertyList(type).Where(selection.IsValid).ToArray();
            if (propertyList.Length == 0)
            {
                return new InjectionRequest(WithoutDependency.Instance, InfuseNothing.Instance);
            }

            var request = new PropertyInjectionRequest(type, propertyList);
            var infusion = operation.CreatePropertyInfusion(request);
            var argumentList = propertyList.Select(info => new Argument(info.Name, info.PropertyType)).ToArray();
            var dependency = new WithDependency(argumentList);
            return new InjectionRequest(dependency, infusion);
        }

        public InjectionRequest GetMethodInjectionRequest(Type type)
        {
            return analysis.GetMethodInjectionRequest(type, createMethodInfusion);
        }
        private InjectionRequest CreateMethodInfusion(Type type)
        {
            var methodList = enumeration.GetMethodList(type).Where(selection.IsValid).ToArray();
            if (methodList.Length == 0)
            {
                return new InjectionRequest(WithoutDependency.Instance, InfuseNothing.Instance);
            }
            if (methodList.Length == 1)
            {
                var request = new MethodInjectionRequest(type, methodList[0]);
                var infusion = operation.CreateMethodInfusion(request);
                if (request.ParameterList.Length == 0)
                {
                    return new InjectionRequest(WithoutDependency.Instance, infusion);
                }

                var argumentList = request.ParameterList.Select(info => new Argument(info.Name, info.ParameterType)).ToArray();
                var dependency = new WithDependency(argumentList);
                return new InjectionRequest(dependency, infusion);
            }

            throw new RagnarokAlreadySelectedException(type, $"Multiple injectable methods in {type}.");
        }
    }
}
