using YggdrAshill.Ragnarok.Periodization;
using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok
{
    public static class ProgressionExtension
    {
        public static ITermination Bind(this IExecution execution, CompositeExecution composite)
        {
            if (execution == null)
            {
                throw new ArgumentNullException(nameof(execution));
            }
            if (composite == null)
            {
                throw new ArgumentNullException(nameof(composite));
            }

            return composite.Bind(execution);
        }
    }
}
