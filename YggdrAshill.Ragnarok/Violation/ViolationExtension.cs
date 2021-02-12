using YggdrAshill.Ragnarok.Progression;
using YggdrAshill.Ragnarok.Violation;
using System;

namespace YggdrAshill.Ragnarok
{
    public static class ViolationExtension
    {
        public static IExecution Bind(this IExecution execution, Action<Exception> abortion)
        {
            if (execution == null)
            {
                throw new ArgumentNullException(nameof(execution));
            }
            if (abortion == null)
            {
                throw new ArgumentNullException(nameof(abortion));
            }

            return execution.Bind(new Abortion(abortion));
        }
    }
}
