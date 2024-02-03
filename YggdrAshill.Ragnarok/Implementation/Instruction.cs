using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class Instruction : IInstruction
    {
        private readonly Action<IObjectResolver> onExecuted;

        public Instruction(Action<IObjectResolver> onExecuted)
        {
            this.onExecuted = onExecuted;
        }

        public void Execute(IObjectResolver resolver)
        {
            onExecuted.Invoke(resolver);
        }
    }
}
