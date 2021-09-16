using YggdrAshill.Ragnarok.Periodization;
using System;

namespace YggdrAshill.Ragnarok.Proceduralization
{
    public static class ProceduralizationExtension
    {
        public static IPlan In(this IExecution execution, Action origination, Action termination)
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

            return execution.In(Periodization.Origination.Of(origination), Periodization.Termination.Of(termination));
        }

        public static IPlan Of(this ISpan span, Action execution)
        {
            if (span is null)
            {
                throw new ArgumentNullException(nameof(span));
            }
            if (execution is null)
            {
                throw new ArgumentNullException(nameof(execution));
            }

            return new Execution(execution).In(span);
        }
    }
}
