using YggdrAshill.Ragnarok.Periodization;

namespace YggdrAshill.Ragnarok.Proceduralization
{
    internal sealed class DelegatedPlan :
        IPlan
    {
        private readonly ISpan span;

        private readonly IExecution execution;

        public DelegatedPlan(ISpan span, IExecution execution)
        {
            this.span = span;

            this.execution = execution;
        }

        /// <inheritdoc/>
        public void Originate()
        {
            span.Originate();
        }

        /// <inheritdoc/>
        public void Terminate()
        {
            span.Terminate();
        }

        /// <inheritdoc/>
        public void Execute()
        {
            execution.Execute();
        }
    }
}
