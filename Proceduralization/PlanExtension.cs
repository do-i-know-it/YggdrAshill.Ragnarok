using YggdrAshill.Ragnarok.Periodization;
using System;

namespace YggdrAshill.Ragnarok.Proceduralization
{
    public static class PlanExtension
    {
        public static void Run(this IPlan plan)
        {
            if (plan is null)
            {
                throw new ArgumentNullException(nameof(plan));
            }

            using (plan.Scope())
            {
                plan.Execute();
            }
        }
    }
}
