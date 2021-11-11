using YggdrAshill.Ragnarok.Periodization;

namespace YggdrAshill.Ragnarok.Specification
{
    internal class FakeCycle :
        ICycle
    {
        private readonly FakeSpan span = new FakeSpan();

        internal bool Originated => span.Originated;

        internal bool Terminated => span.Terminated;

        public ISpan Span => span;

        private readonly FakeExecution execution = new FakeExecution();

        internal bool Executed => execution.Executed;

        public IExecution Execution => execution;
    }
}
