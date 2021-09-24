using YggdrAshill.Ragnarok.Periodization;

namespace YggdrAshill.Ragnarok.Specification
{
    internal class FakeOrigination :
        IOrigination
    {
        internal bool Originated { get; private set; }

        public void Originate()
        {
            Originated = true;
        }
    }
}
