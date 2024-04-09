using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ActivateWithFunction : IActivation
    {
        private readonly Func<object[], object> onActivated;

        public ActivateWithFunction(Func<object[], object> onActivated)
        {
            this.onActivated = onActivated;
        }

        public object Activate(object[] parameterList)
        {
            return onActivated.Invoke(parameterList);
        }
    }
}
