using YggdrAshill.Ragnarok.Periodization;
using System;
using System.Linq;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Experimental
{
    public sealed class PeriodizedService :
        IService
    {
        public static PeriodizedService Default { get; }
            = new PeriodizedService(new IOrigination[0], new ITermination[0], new IExecution[0], new ISpan[0]);

        private readonly IEnumerable<IOrigination> originations;

        private readonly IEnumerable<ITermination> terminations;

        private readonly IEnumerable<IExecution> executions;

        private readonly IEnumerable<ISpan> spans;

        private PeriodizedService(
            IEnumerable<IOrigination> originations,
            IEnumerable<ITermination> terminations,
            IEnumerable<IExecution> executions,
            IEnumerable<ISpan> spans)
        {
            this.originations = originations;

            this.terminations = terminations;

            this.executions = executions;

            this.spans = spans;
        }

        public PeriodizedService Configure(IOrigination origination)
        {
            if (origination is null)
            {
                throw new ArgumentNullException(nameof(origination));
            }

            var added = originations.Append(origination);

            return new PeriodizedService(added, terminations, executions, spans);
        }

        public PeriodizedService Configure(ITermination termination)
        {
            if (termination is null)
            {
                throw new ArgumentNullException(nameof(termination));
            }

            var added = terminations.Append(termination);

            return new PeriodizedService(originations, added, executions, spans);
        }

        public PeriodizedService Configure(IExecution execution)
        {
            if (execution is null)
            {
                throw new ArgumentNullException(nameof(execution));
            }

            var added = executions.Append(execution);

            return new PeriodizedService(originations, terminations, added, spans);
        }

        public PeriodizedService Configure(ISpan span)
        {
            if (span is null)
            {
                throw new ArgumentNullException(nameof(span));
            }

            var added = spans.Append(span);

            return new PeriodizedService(originations, terminations, executions, added);
        }

        public ICycle Build()
        {
            return new Cycle(originations.ToArray(), terminations.ToArray(), executions.ToArray(), spans.ToArray());
        }

        private sealed class Cycle :
            ICycle
        {
            private readonly IOrigination[] originations;

            private readonly ITermination[] terminations;

            private readonly IExecution[] executions;

            private readonly ISpan[] spans;

            internal Cycle(
                IOrigination[] originations,
                ITermination[] terminations,
                IExecution[] executions,
                ISpan[] spans)
            {
                this.originations = originations;

                this.terminations = terminations;

                this.executions = executions;

                this.spans = spans;
            }

            public void Originate()
            {
                foreach (var origination in originations)
                {
                    origination.Originate();
                }

                foreach (var span in spans)
                {
                    span.Originate();
                }
            }

            public void Terminate()
            {
                foreach (var span in spans.Reverse())
                {
                    span.Terminate();
                }

                foreach (var termination in terminations)
                {
                    termination.Terminate();
                }
            }

            public void Execute()
            {
                foreach (var execution in executions)
                {
                    execution.Execute();
                }
            }
        }
    }
}