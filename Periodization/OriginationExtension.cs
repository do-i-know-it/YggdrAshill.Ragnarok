using System;

namespace YggdrAshill.Ragnarok.Periodization
{
    public static class OriginationExtension
    {
        public static ISpan To(this IOrigination origination, ITermination termination)
        {
            if (origination is null)
            {
                throw new ArgumentNullException(nameof(origination));
            }
            if (termination is null)
            {
                throw new ArgumentNullException(nameof(termination));
            }

            return new DelegatedSpan(origination, termination);
        }
    }
}
