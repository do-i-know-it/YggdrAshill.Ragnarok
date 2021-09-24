using System;

namespace YggdrAshill.Ragnarok.Periodization
{
    public static class CycleExtension
    {
        public static void Run(this ICycle cycle)
        {
            if (cycle is null)
            {
                throw new ArgumentNullException(nameof(cycle));
            }

            cycle.Originate();

            try
            {
                cycle.Execute();
            }
            finally
            {
                cycle.Terminate();
            }
        }
    }
}
