using System;

namespace YggdrAshill.Ragnarok.Periodization
{
    /// <summary>
    /// Defines extensions for <see cref="ISpan"/>.
    /// </summary>
    public static class SpanExtension
    {
        /// <summary>
        /// Opens <see cref="ISpan"/>.
        /// </summary>
        /// <param name="span">
        /// <see cref="ISpan"/> to open.
        /// </param>
        /// <returns>
        /// <see cref="IDisposable"/> to close <see cref="ISpan"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="span"/> is null.
        /// </exception>
        public static IDisposable Open(this ISpan span)
        {
            if (span is null)
            {
                throw new ArgumentNullException(nameof(span));
            }

            span.Origination.Originate();

            return new DisposeToTerminate(span.Termination);
        }
        private sealed class DisposeToTerminate :
            IDisposable
        {
            private readonly ITermination termination;

            internal DisposeToTerminate(ITermination termination)
            {
                this.termination = termination;
            }

            public void Dispose()
            {
                termination.Terminate();
            }
        }
    }
}
