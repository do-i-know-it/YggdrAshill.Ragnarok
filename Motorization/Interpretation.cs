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
        private readonly Func<Type, InjectionRequest> createFieldInfusion;
        private readonly Func<Type, InjectionRequest> createPropertyInfusion;
        private readonly Func<Type, InjectionRequest> createMethodInfusion;

        public Interpretation(IDependencyEnumeration enumeration, IDependencySelection selection, IDependencyOperation operation)
        {
            this.enumeration = enumeration;
            this.selection = selection;
            this.operation = operation;

            createActivation = CreateActivation;
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
                if (openedGenericType == typeof(IEnumerable<>) ||
                    openedGenericType == typeof(IReadOnlyCollection<>) ||
                    openedGenericType == typeof(IReadOnlyList<>))
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
            var activation = GetInstantiationRequest(type).Activation;
            return new ServiceBundleDescription(type, activation, collection);
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

            var selectedConstructor = default(ConstructorInfo);
            var defaultConstructor = default(ConstructorInfo);
            foreach (var info in enumeration.GetConstructorList(type))
            {
                if (selection.IsValid(info))
                {
                    if (selectedConstructor != null)
                    {
                        throw new RagnarokAlreadySelectedException(type, $"Multiple injectable constructors in {type}.");
                    }

                    selectedConstructor = info;
                    continue;
                }

                if (defaultConstructor == null)
                {
                    defaultConstructor = info;
                    continue;
                }

                if (defaultConstructor.GetParameters().Length < info.GetParameters().Length)
                {
                    defaultConstructor = info;
                }
            }
            if (selectedConstructor != null)
            {
                var request = new ConstructorInjectionRequest(type, selectedConstructor);
                var activation = request.ParameterList.Length == 0 ? operation.CreateActivation(type) : operation.CreateActivation(request);
                return new InstantiationRequest(request.Dependency, activation);
            }
            if (defaultConstructor != null)
            {
                var request = new ConstructorInjectionRequest(type, defaultConstructor);
                var activation = request.ParameterList.Length == 0 ? operation.CreateActivation(type) : operation.CreateActivation(request);
                return new InstantiationRequest(request.Dependency, activation);
            }
            else
            {
                var activation = operation.CreateActivation(type);
                return new InstantiationRequest(WithoutDependency.Instance, activation);
            }
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
            return new InjectionRequest(request.Dependency, infusion);
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
            return new InjectionRequest(request.Dependency, infusion);
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
                var infusion = request.ParameterList.Length == 0 ? InfuseNothing.Instance : operation.CreateMethodInfusion(request);
                return new InjectionRequest(request.Dependency, infusion);
            }

            throw new RagnarokAlreadySelectedException(type, $"Multiple injectable methods in {type}.");
        }
    }
}
