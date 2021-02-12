using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok.Violation
{
    public static class ViolationExtension
    {
        public static IExecution Bind(this IExecution execution, IAbortion abortion)
        {
            if (execution == null)
            {
                throw new ArgumentNullException(nameof(execution));
            }
            if (abortion == null)
            {
                throw new ArgumentNullException(nameof(abortion));
            }

            return new ExecutionWithAbortion(execution, abortion);
        }
    }
}
