using YggdrAshill.Ragnarok.Periodization;

namespace YggdrAshill.Ragnarok.Specification
{
    internal class FakeExecution :
        IExecution
    {
        internal bool Executed { get; private set; }

        public void Execute()
        {
            Executed = true;
        }
    }
}
