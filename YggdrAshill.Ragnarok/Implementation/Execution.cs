using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class Execution : IExecution
    {
        private readonly Action<IObjectResolver> onExecuted;

        public Execution(Action<IObjectResolver> onExecuted)
        {
            this.onExecuted = onExecuted;
        }

        public void Execute(IObjectResolver resolver)
        {
            onExecuted.Invoke(resolver);
        }
    }
}
