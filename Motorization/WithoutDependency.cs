using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class WithoutDependency : IDependency, IRealization
    {
        public static WithoutDependency Instance { get; } = new();

        private WithoutDependency()
        {

        }

        public IReadOnlyList<Type> DependentTypeList { get; } = Array.Empty<Type>();

        public IRealization CreateRealization(IReadOnlyList<IParameter> parameterList)
        {
            return this;
        }

        public object[] Realize(IObjectResolver resolver)
        {
            return Array.Empty<object>();
        }
    }
}
