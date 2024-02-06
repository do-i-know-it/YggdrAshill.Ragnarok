using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ActivateWithFunction : IActivation
    {
        private readonly Func<object[], object> onActivated;

        public IDependency Dependency { get; }

        public ActivateWithFunction(Func<object[], object> onActivated, IDependency dependency)
        {
            this.onActivated = onActivated;

            Dependency = dependency;
        }

        public object Activate(object[] parameterList)
        {
            return onActivated.Invoke(parameterList);
        }
    }
}
