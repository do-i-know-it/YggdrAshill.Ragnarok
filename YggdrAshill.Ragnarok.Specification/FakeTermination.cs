using YggdrAshill.Ragnarok.Periodization;

namespace YggdrAshill.Ragnarok.Specification
{
    internal class FakeTermination :
        ITermination
    {
        internal bool Terminated { get; private set; }

        public void Terminate()
        {
            Terminated = true;
        }
    }
}
