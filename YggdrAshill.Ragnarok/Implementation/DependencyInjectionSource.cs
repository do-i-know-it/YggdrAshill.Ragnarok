using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class DependencyInjectionSource : IDependencyInjection
    {
        private readonly Type implementedType;
        private readonly ICompilation compilation;
        private readonly InstanceInjectionSource source;

        public DependencyInjectionSource(Type implementedType, IObjectContainer container)
        {
            this.implementedType = implementedType;
            compilation = container.Compilation;
            source = new InstanceInjectionSource(implementedType, container);
        }

        private List<IParameter>? parameterList;

        public IInstantiation CreateInstantiation()
        {
            var request = parameterList == null ?
                new DependencyInjectionRequest(implementedType) :
                new DependencyInjectionRequest(implementedType, parameterList.ToArray());
            var instantiation = compilation.CreateInstantiation(request);

            if (!source.CanInjectIntoInstance(out var injection))
            {
                return instantiation;
            }

            return injection.ToInstantiate(instantiation);
        }

        public Type ImplementedType => source.ImplementedType;

        public IReadOnlyList<Type> AssignedTypeList => source.AssignedTypeList;

        public IDependencyInjection WithArgument(IParameter parameter)
        {
            if (parameterList == null)
            {
                parameterList = new List<IParameter>();
            }

            if (!parameterList.Contains(parameter))
            {
                parameterList.Add(parameter);
            }

            return this;
        }

        public void AsOwnSelf()
        {
            source.AsOwnSelf();
        }

        public IInheritedTypeAssignment As(Type inheritedType)
        {
            return source.As(inheritedType);
        }

        public IOwnTypeAssignment AsImplementedInterfaces()
        {
            return source.AsImplementedInterfaces();
        }

        public IParameterMethodInjection WithMethod(IParameter parameter)
        {
            return source.WithMethod(parameter);
        }

        public ITypeAssignment WithMethodInjection()
        {
            return source.WithMethodInjection();
        }

        public IParameterPropertyInjection WithProperty(IParameter parameter)
        {
            return source.WithProperty(parameter);
        }

        public IMethodInjection WithPropertyInjection()
        {
            return source.WithPropertyInjection();
        }

        public IParameterFieldInjection WithField(IParameter parameter)
        {
            return source.WithField(parameter);
        }

        public IPropertyInjection WithFieldInjection()
        {
            return source.WithFieldInjection();
        }

        public IFieldInjection ResolvedImmediately()
        {
            return source.ResolvedImmediately();
        }
    }
}
