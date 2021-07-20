using YggdrAshill.Ragnarok.Progression;

namespace YggdrAshill.Ragnarok.Specification
{
    internal class FakeProcess :
        IProcess
    {
        private readonly FakePeriod period = new FakePeriod();

        private readonly FakeExecution execution = new FakeExecution();

        internal bool Originated => period.Originated;

        internal bool Terminated => period.Terminated;

        internal bool Executed => execution.Executed;

        public void Originate()
        {
            period.Originate();
        }

        public void Terminate()
        {
            period.Terminate();
        }

        public void Execute()
        {
            execution.Execute();
        }
    }
}
