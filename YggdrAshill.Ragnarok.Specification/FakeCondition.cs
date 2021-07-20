using YggdrAshill.Ragnarok.Progression;

namespace YggdrAshill.Ragnarok.Specification
{
    internal class FakeCondition :
        ICondition
    {
        internal FakeCondition(bool isSatisfied)
        {
            IsSatisfied = isSatisfied;
        }

        public bool IsSatisfied { get; }
    }
}
