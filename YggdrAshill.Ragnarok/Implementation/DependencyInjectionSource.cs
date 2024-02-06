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

        public DependencyInjectionSource(Type implementedType, ICompilation compilation)
        {
            this.implementedType = implementedType;
            this.compilation = compilation;
            source = new InstanceInjectionSource(implementedType, compilation);
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

        public IMethodInjection WithMethod(IParameter parameter)
        {
            return source.WithMethod(parameter);
        }

        public IMethodInjection WithMethodInjection()
        {
            return source.WithMethodInjection();
        }

        public IPropertyInjection WithProperty(IParameter parameter)
        {
            return source.WithProperty(parameter);
        }

        public IPropertyInjection WithPropertyInjection()
        {
            return source.WithPropertyInjection();
        }

        public IFieldInjection WithField(IParameter parameter)
        {
            return source.WithField(parameter);
        }

        public IFieldInjection WithFieldInjection()
        {
            return source.WithFieldInjection();
        }
    }
}
