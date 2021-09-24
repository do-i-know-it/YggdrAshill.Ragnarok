namespace YggdrAshill.Ragnarok.Periodization
{
    internal sealed class Cycle :
        ICycle
    {
        private readonly IOrigination origination;

        private readonly ITermination termination;

        private readonly IExecution execution;

        internal Cycle(IOrigination origination, ITermination termination, IExecution execution)
        {
            this.origination = origination;

            this.termination = termination;

            this.execution = execution;
        }

        public void Originate()
        {
            origination.Originate();
        }

        public void Terminate()
        {
            termination.Terminate();
        }

        public void Execute()
        {
            execution.Execute();
        }
    }
}
