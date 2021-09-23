using YggdrAshill.Ragnarok.Periodization;

namespace YggdrAshill.Ragnarok.Proceduralization
{
    internal sealed class Cycle :
        ICycle
    {
        private readonly IOrigination origination;

        private readonly ITermination termination;

        private readonly IExecution execution;

        public Cycle(IOrigination origination, ITermination termination, IExecution execution)
        {
            this.origination = origination;

            this.termination = termination;

            this.execution = execution;
        }

        /// <inheritdoc/>
        public void Originate()
        {
            origination.Originate();
        }

        /// <inheritdoc/>
        public void Terminate()
        {
            termination.Terminate();
        }

        /// <inheritdoc/>
        public void Execute()
        {
            execution.Execute();
        }
    }
}
