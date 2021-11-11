using YggdrAshill.Ragnarok.Periodization;
using System.Linq;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Experimental
{
    internal static class SpanBuilder
    {
        internal static ISpanBuilder Default { get; } = new None();
        private sealed class None :
            IOrigination,
            ITermination,
            ISpan,
            ISpanBuilder
        {
            public ISpanBuilder Configure(ISpan span)
            {
                return new One(span);
            }

            public ISpan Build()
            {
                return this;
            }

            public IOrigination Origination => this;

            public ITermination Termination => this;

            public void Originate()
            {

            }

            public void Terminate()
            {

            }
        }

        private sealed class One :
            ISpanBuilder
        {
            private readonly ISpan first;

            internal One(ISpan first)
            {
                this.first = first;
            }

            public ISpan Build()
            {
                return first;
            }

            public ISpanBuilder Configure(ISpan span)
            {
                if (first == span)
                {
                    return this;
                }

                return new Listed(new[] { first, span });
            }
        }

        internal sealed class Listed :
            ISpanBuilder
        {
            private readonly IEnumerable<ISpan> spans;

            internal Listed(IEnumerable<ISpan> spans)
            {
                this.spans = spans;
            }

            public ISpan Build()
            {
                return new Span(spans.ToArray());
            }

            public ISpanBuilder Configure(ISpan span)
            {
                if (spans.Contains(span))
                {
                    return this;
                }

                return new Listed(spans.Append(span));
            }
        }
        private sealed class Span :
            IOrigination,
            ITermination,
            ISpan
        {
            private readonly IOrigination[] originations;

            private readonly ITermination[] terminations;

            internal Span(IEnumerable<ISpan> spans)
            {
                originations
                    = spans
                    .Select(span => span.Origination)
                    .ToArray();

                terminations
                    = spans
                    .Reverse()
                    .Select(span => span.Termination)
                    .ToArray();
            }

            public IOrigination Origination => this;

            public ITermination Termination => this;

            public void Originate()
            {
                foreach (var origination in originations)
                {
                    origination.Originate();
                }
            }

            public void Terminate()
            {
                foreach (var termination in terminations)
                {
                    termination.Terminate();
                }
            }
        }
    }
}
