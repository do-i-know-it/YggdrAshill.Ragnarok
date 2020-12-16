using YggdrAshill.Ragnarok.Administration;
using System;
using YggdrAshill.Ragnarok.Periodization;

namespace YggdrAshill.Ragnarok
{
    public static class AdministrationExtension
    {
        public static void Collect(this ITermination termination, ITerminationCollection collection)
        {
            if (termination == null)
            {
                throw new ArgumentNullException(nameof(termination));
            }
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            collection.Collect(termination);
        }

        public static void Collect(this IExecution execution, IExecutionCollection collection)
        {
            if (execution == null)
            {
                throw new ArgumentNullException(nameof(execution));
            }
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            collection.Collect(execution);
        }
    }
}
