namespace YggdrAshill.Ragnarok.Periodization
{
    internal sealed class Span :
        ISpan
    {
        public IOrigination Origination { get; }

        public ITermination Termination { get; }

        internal Span(IOrigination origination, ITermination termination)
        {
            Origination = origination;

            Termination = termination;
        }
    }
}
