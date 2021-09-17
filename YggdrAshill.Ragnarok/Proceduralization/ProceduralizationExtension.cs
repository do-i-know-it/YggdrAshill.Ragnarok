using YggdrAshill.Ragnarok.Proceduralization;
using System;

namespace YggdrAshill.Ragnarok
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

            return execution.In(Origination.Of(origination), Termination.Of(termination));
        }
    }
}
