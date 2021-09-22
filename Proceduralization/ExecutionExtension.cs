using YggdrAshill.Ragnarok.Periodization;
using System;

namespace YggdrAshill.Ragnarok.Proceduralization
{
    public static class ExecutionExtension
    {
        public static IPlan In(this IExecution execution, ISpan span)
        {
            if (execution is null)
            {
                throw new ArgumentNullException(nameof(execution));
            }
            if (span is null)
            {
                throw new ArgumentNullException(nameof(span));
            }

            return new DelegatedPlan(span, execution);
        }

        public static IPlan Between(this IExecution execution, IOrigination origination, ITermination termination)
        {
            if (execution is null)
            {
                throw new ArgumentNullException(nameof(execution));
            }
            if (origination is null)
            {
                throw new ArgumentNullException(nameof(origination));
            }
            if (termination is null)
            {
                throw new ArgumentNullException(nameof(termination));
            }

            return execution.In(origination.To(termination));
        }
    }
}
