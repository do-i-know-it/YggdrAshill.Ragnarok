using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class DependencyInjectionSource
    {
        private readonly Type implementedType;
        private readonly ICompilation compilation;

        public DependencyInjectionSource(Type implementedType, ICompilation compilation)
        {
            this.implementedType = implementedType;
            this.compilation = compilation;
        }

        private List<IParameter>? parameterList;

        public IInstantiation CreateInstantiation()
        {
            var activation = compilation.CreateActivation(implementedType);

            return parameterList == null
                ? new ActivateToInstantiateWithoutParameterList(activation)
                : new ActivateToInstantiateWithParameterList(activation, parameterList);
        }

        public void AddArgument(IParameter parameter)
        {
            if (parameterList == null)
            {
                parameterList = new List<IParameter>();
            }

            if (!parameterList.Contains(parameter))
            {
                parameterList.Add(parameter);
            }
        }
    }
}
