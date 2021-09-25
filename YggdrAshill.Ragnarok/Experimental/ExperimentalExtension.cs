using System;

namespace YggdrAshill.Ragnarok.Experimental
{
    public static class ExperimentalExtension
    {
        public static IService OnOriginated(this IService service, Action origination)
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

        public static IService OnTerminated(this IService service, Action termination)
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

        public static IService OnExecuted(this IService service, Action execution)
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

        public static IService InSpan(this IService service, Action origination, Action termination)
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

            return service.Configure(Origination.Of(origination).To(termination));
        }
    }
}
