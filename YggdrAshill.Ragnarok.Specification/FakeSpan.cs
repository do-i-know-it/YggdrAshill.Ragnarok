using YggdrAshill.Ragnarok.Periodization;

namespace YggdrAshill.Ragnarok.Specification
{
    internal class FakeSpan :
        ISpan
    {
        private readonly FakeOrigination origination = new FakeOrigination();

        internal bool Originated => origination.Originated;

        public IOrigination Origination => origination;

        private readonly FakeTermination termination = new FakeTermination();

        internal bool Terminated => termination.Terminated;

        public ITermination Termination => termination;
    }
}
