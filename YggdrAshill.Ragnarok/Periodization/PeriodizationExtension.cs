using YggdrAshill.Ragnarok.Periodization;
using System;

namespace YggdrAshill.Ragnarok
{
    public static class PeriodizationExtension
    {
        public static void Bind(this ITermination termination, CompositeTermination composite)
        {
            if (termination == null)
            {
                throw new ArgumentNullException(nameof(termination));
            }
            if (composite == null)
            {
                throw new ArgumentNullException(nameof(composite));
            }

            composite.Bind(termination);
        }
    }
}
