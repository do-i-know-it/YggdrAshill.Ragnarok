using YggdrAshill.Ragnarok.Materialization;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class InfuseWithAction :
        IInfusion
    {
        private readonly Action<object, object[]> onInfused;
        public IReadOnlyList<Argument> ArgumentList { get; }

        public InfuseWithAction(Action<object, object[]> onInfused, IReadOnlyList<Argument> argumentList)
        {
            this.onInfused = onInfused;
            ArgumentList = argumentList;
        }
        public void Infuse(object instance, object[] parameterList)
        {
            onInfused.Invoke(instance, parameterList);
        }
    }
}
