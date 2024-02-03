using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class InfuseNothing : IInfusion, IInfusionV2
    {
        public static InfuseNothing Instance { get; } = new();

        private InfuseNothing()
        {

        }

        public IReadOnlyList<Argument> ArgumentList { get; } = Array.Empty<Argument>();

        public IDependency Dependency => WithoutDependency.Instance;

        public void Infuse(object instance, object[] parameterList)
        {

        }
    }
}
