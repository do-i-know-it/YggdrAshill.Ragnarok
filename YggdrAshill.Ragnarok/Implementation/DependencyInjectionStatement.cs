using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class DependencyInjectionStatement :
        IDependencyInjection,
        IStatement
    {
        private readonly ICompilation compilation;
        private readonly InstanceInjection instanceInjection;

        public DependencyInjectionStatement(ICompilation compilation, Type implementedType)
        {
            this.compilation = compilation;

            instanceInjection = new InstanceInjection(compilation, implementedType);
        }

        private readonly List<IParameter> parameterList = new List<IParameter>();

        public Type ImplementedType => instanceInjection.ImplementedType;
        public IReadOnlyList<Type> AssignedTypeList => instanceInjection.AssignedTypeList;

        private IInstantiation? instantiation;
        public IInstantiation Instantiation
        {
            get
            {
                if (instantiation == null)
                {
                    instantiation = compilation.GetInstantiation(instanceInjection.ImplementedType, parameterList);

                    var injection = instanceInjection.GetInjection();

                    if (injection != null)
                    {
                        instantiation = new InstantiateWithInjection(instantiation, injection);
                    }
                }

                return instantiation;
            }
        }

        public void AsSelf()
        {
            instanceInjection.AsSelf();
        }

        public IAssignImplementedInterface As(Type implementedInterface)
        {
            return instanceInjection.As(implementedInterface);
        }

        public IAssignImplementedType AsImplementedInterfaces()
        {
            return instanceInjection.AsImplementedInterfaces();
        }

        public IInjectIntoMethodExternally WithMethodInjected()
        {
            return instanceInjection.WithMethodInjected();
        }

        public IInjectIntoPropertiesExternally WithPropertiesInjected()
        {
            return instanceInjection.WithPropertiesInjected();
        }

        public IInjectIntoFieldsExternally WithFieldsInjected()
        {
            return instanceInjection.WithFieldsInjected();
        }

        public IDependencyInjection WithArgument(IParameter parameter)
        {
            if (!parameterList.Contains(parameter))
            {
                parameterList.Add(parameter);
            }

            return this;
        }
    }
}
