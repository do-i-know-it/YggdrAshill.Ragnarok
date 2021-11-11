using YggdrAshill.Ragnarok.Periodization;
using System;

namespace YggdrAshill.Ragnarok.Experimental
{
    public sealed class Service :
        IService
    {
        public static Service Default { get; }
            = new Service(OriginationBuilder.Default, TerminationBuilder.Default, ExecutionBuilder.Default, SpanBuilder.Default);

        private readonly IOriginationBuilder originationBuilder;

        private readonly ITerminationBuilder terminationBuilder;

        private readonly IExecutionBuilder executionBuilder;

        private readonly ISpanBuilder spanBuilder;

        private Service(
            IOriginationBuilder originationBuilder,
            ITerminationBuilder terminationBuilder,
            IExecutionBuilder executionBuilder,
            ISpanBuilder spanBuilder)
        {
            this.originationBuilder = originationBuilder;

            this.terminationBuilder = terminationBuilder;

            this.executionBuilder = executionBuilder;

            this.spanBuilder = spanBuilder;
        }

        public IService Configure(IOrigination origination)
        {
            if (origination is null)
            {
                throw new ArgumentNullException(nameof(origination));
            }

            return new Service(originationBuilder.Configure(origination), terminationBuilder, executionBuilder, spanBuilder);
        }

        public IService Configure(ITermination termination)
        {
            if (termination is null)
            {
                throw new ArgumentNullException(nameof(termination));
            }

            return new Service(originationBuilder, terminationBuilder.Configure(termination), executionBuilder, spanBuilder);
        }

        public IService Configure(IExecution execution)
        {
            if (execution is null)
            {
                throw new ArgumentNullException(nameof(execution));
            }

            return new Service(originationBuilder, terminationBuilder, executionBuilder.Configure(execution), spanBuilder);
        }

        public IService Configure(ISpan span)
        {
            if (span is null)
            {
                throw new ArgumentNullException(nameof(span));
            }

            return new Service(originationBuilder, terminationBuilder, executionBuilder, spanBuilder.Configure(span));
        }

        public ICycle Build()
        {
            var origination = originationBuilder.Build();

            var termination = terminationBuilder.Build();

            var execution = executionBuilder.Build();

            var span = spanBuilder.Build();

            var cycle = execution.Between(origination, termination);

            return cycle.In(span);
        }
    }
}
