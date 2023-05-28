using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class InstanceInjectionStatement :
        IInstanceInjection,
        IStatement
    {
        private readonly InstanceInjection instanceInjection;
        private readonly IInstantiation instantiation;

        public InstanceInjectionStatement(ICompilation compilation, Type implementedType, IInstantiation instantiation)
        {
            instanceInjection = new InstanceInjection(compilation, implementedType);
            this.instantiation = instantiation;
        }

        public Type ImplementedType => instanceInjection.ImplementedType;
        public IReadOnlyList<Type> AssignedTypeList => instanceInjection.AssignedTypeList;

        private IInstantiation? cache;
        public IInstantiation Instantiation
        {
            get
            {
                if (cache == null)
                {
                    var injection = instanceInjection.GetInjection();

                    if (injection == null)
                    {
                        cache = instantiation;
                    }
                    else
                    {
                        cache = new InstantiateWithInjection(instantiation, injection);
                    }
                }

                return cache;
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
    }
}
