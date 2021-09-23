namespace YggdrAshill.Ragnarok.Periodization
{
    internal sealed class Span :
        ISpan
    {
        private readonly IOrigination origination;

        private readonly ITermination termination;

        internal Span(IOrigination origination, ITermination termination)
        {
            this.origination = origination;

            this.termination = termination;
        }

        /// <inheritdoc/>
        public void Originate()
        {
            origination.Originate();
        }

        /// <inheritdoc/>
        public void Terminate()
        {
            termination.Terminate();
        }
    }
}
