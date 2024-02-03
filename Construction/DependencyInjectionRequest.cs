using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    public sealed class DependencyInjectionRequest
    {
        public Type ImplementedType { get; }
        public IReadOnlyList<IParameter> ParameterList { get; }

        public DependencyInjectionRequest(Type implementedType, params IParameter[] parameterList)
        {
            ImplementedType = implementedType;
            ParameterList = parameterList;
        }
    }
}
