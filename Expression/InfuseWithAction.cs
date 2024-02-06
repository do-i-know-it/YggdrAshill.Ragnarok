using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class InfuseWithAction : IInfusion
    {
        private readonly Action<object, object[]> onInfused;

        public IDependency Dependency { get; }

        public InfuseWithAction(Action<object, object[]> onInfused, IDependency dependency)
        {
            this.onInfused = onInfused;
            Dependency = dependency;
        }

        public void Infuse(object instance, object[] parameterList)
        {
            onInfused.Invoke(instance, parameterList);
        }
    }
}
