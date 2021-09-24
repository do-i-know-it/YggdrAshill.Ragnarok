using YggdrAshill.Ragnarok.Periodization;
using System;

namespace YggdrAshill.Ragnarok.Experimental
{
    public static class PeriodizedServiceExtension
    {
        public static PeriodizedService In(this PeriodizedService service, IOrigination origination, ITermination termination)
        {
            if (service is null)
            {
                throw new ArgumentNullException(nameof(origination));
            }
            if (origination is null)
            {
                throw new ArgumentNullException(nameof(origination));
            }
            if (termination is null)
            {
                throw new ArgumentNullException(nameof(termination));
            }

            return service.Configure(origination.To(termination));
        }
    }
}
