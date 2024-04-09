using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class InfuseWithAction : IInfusion
    {
        private readonly Action<object, object[]> onInfused;

        public InfuseWithAction(Action<object, object[]> onInfused)
        {
            this.onInfused = onInfused;
        }

        public void Infuse(object instance, object[] parameterList)
        {
            onInfused.Invoke(instance, parameterList);
        }
    }
}
