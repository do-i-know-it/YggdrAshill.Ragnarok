using YggdrAshill.Ragnarok.Periodization;
using System.Linq;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Experimental
{
    internal static class SpanBuilder
    {
        internal static ISpanBuilder Default { get; } = new None();
        private sealed class None :
            ISpanBuilder,
            ISpan
        {
            public ISpan Build()
            {
                return this;
            }

            public ISpanBuilder Configure(ISpan span)
            {
                return new One(span);
            }

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
            ISpan
        {
            private readonly ISpan[] spans;

            internal Span(ISpan[] spans)
            {
                this.spans = spans;
            }

            public void Originate()
            {
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
            }
        }
    }
}
