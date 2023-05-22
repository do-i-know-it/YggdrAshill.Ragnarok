using YggdrAshill.Ragnarok.Materialization;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Reflection
{
    internal sealed class ActivateWithFunction :
        IActivation
    {
        private readonly Func<object[], object> onActivated;

        public IReadOnlyList<Argument> ArgumentList { get; }

        public ActivateWithFunction(Func<object[], object> onActivated, IReadOnlyList<Argument> argumentList)
        {
            this.onActivated = onActivated;

            ArgumentList = argumentList;
        }

        public object Activate(object[] parameterList)
        {
            return onActivated.Invoke(parameterList);
        }
    }
}
