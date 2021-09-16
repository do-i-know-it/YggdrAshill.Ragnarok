namespace YggdrAshill.Ragnarok.Periodization
{
    internal sealed class DelegatedSpan :
        ISpan
    {
        private readonly IOrigination origination;

        private readonly ITermination termination;

        internal DelegatedSpan(IOrigination origination, ITermination termination)
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
