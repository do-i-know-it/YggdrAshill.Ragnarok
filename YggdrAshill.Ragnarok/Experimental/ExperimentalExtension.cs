using System;

namespace YggdrAshill.Ragnarok.Experimental
{
    public static class ExperimentalExtension
    {
        public static PeriodizedService OnOriginated(this PeriodizedService service, Action origination)
        {
            if (service is null)
            {
                throw new ArgumentNullException(nameof(origination));
            }
            if (origination is null)
            {
                throw new ArgumentNullException(nameof(origination));
            }

            return service.Configure(Origination.Of(origination));
        }

        public static PeriodizedService OnTerminated(this PeriodizedService service, Action termination)
        {
            if (service is null)
            {
                throw new ArgumentNullException(nameof(termination));
            }
            if (termination is null)
            {
                throw new ArgumentNullException(nameof(termination));
            }

            return service.Configure(Termination.Of(termination));
        }

        public static PeriodizedService OnExecuted(this PeriodizedService service, Action execution)
        {
            if (service is null)
            {
                throw new ArgumentNullException(nameof(execution));
            }
            if (execution is null)
            {
                throw new ArgumentNullException(nameof(execution));
            }

            return service.Configure(Execution.Of(execution));
        }

        public static PeriodizedService In(this PeriodizedService service, Action origination, Action termination)
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

            return service.In(Origination.Of(origination), Termination.Of(termination));
        }
    }
}
