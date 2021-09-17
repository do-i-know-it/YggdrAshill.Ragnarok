using YggdrAshill.Ragnarok.Periodization;
using System;

namespace YggdrAshill.Ragnarok
{
    public static class PeriodizationExtension
    {
        public static ISpan To(this IOrigination origination, Action termination)
        {
            if (origination is null)
            {
                throw new ArgumentNullException(nameof(origination));
            }
            if (termination is null)
            {
                throw new ArgumentNullException(nameof(termination));
            }

            return origination.To(new Termination(termination));
        }

        public static ISpan From(this ITermination termination, Action origination)
        {
            if (termination is null)
            {
                throw new ArgumentNullException(nameof(termination));
            }
            if (origination is null)
            {
                throw new ArgumentNullException(nameof(origination));
            }

            return termination.From(new Origination(origination));
        }
    }
}
