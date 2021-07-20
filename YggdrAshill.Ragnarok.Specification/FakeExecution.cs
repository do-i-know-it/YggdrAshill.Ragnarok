using YggdrAshill.Ragnarok.Progression;

namespace YggdrAshill.Ragnarok.Specification
{
    internal class FakeExecution :
        IExecution
    {
        internal bool Executed { get; set; } = false;

        public void Execute()
        {
            Executed = true;
        }
    }
}
