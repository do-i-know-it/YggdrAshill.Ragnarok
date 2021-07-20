using YggdrAshill.Ragnarok.Progression;

namespace YggdrAshill.Ragnarok.Specification
{
    internal class FakePeriod :
        IPeriod
    {
        private readonly FakeOrigination origination = new FakeOrigination();

        private readonly FakeTermination termination = new FakeTermination();

        internal bool Originated => origination.Originated;

        internal bool Terminated => termination.Terminated;

        public void Originate()
        {
            origination.Originate();
        }

        public void Terminate()
        {
            termination.Terminate();
        }
    }
}
