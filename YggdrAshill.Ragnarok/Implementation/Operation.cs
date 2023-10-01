using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class Operation : IOperation
    {
        private readonly Action<IObjectResolver> onOperated;

        public Operation(Action<IObjectResolver> onOperated)
        {
            this.onOperated = onOperated;
        }

        public void Operate(IObjectResolver resolver)
        {
            onOperated.Invoke(resolver);
        }
    }
}
