using System;

namespace YggdrAshill.Ragnarok.Periodization
{
    public static class TerminationExtension
    {
        public static ISpan From(this ITermination termination, IOrigination origination)
        {
            if (termination is null)
            {
                throw new ArgumentNullException(nameof(termination));
            }
            if (origination is null)
            {
                throw new ArgumentNullException(nameof(origination));
            }

            return new Span(origination, termination);
        }
    }
}
