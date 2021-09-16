using System;

namespace YggdrAshill.Ragnarok.Periodization
{
    public static class SpanExtension
    {
        public static IDisposable Scope(this ISpan span)
        {
            if (span is null)
            {
                throw new ArgumentNullException(nameof(span));
            }

            span.Originate();

            return new DisposeToTerminate(span);
        }
        private sealed class DisposeToTerminate :
            IDisposable
        {
            private readonly ITermination termination;

            internal DisposeToTerminate(ITermination termination)
            {
                this.termination = termination;
            }

            /// <inheritdoc/>
            public void Dispose()
            {
                termination.Terminate();
            }
        }
    }
}
