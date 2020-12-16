using YggdrAshill.Ragnarok.Periodization;
using YggdrAshill.Ragnarok.Progression;
using YggdrAshill.Ragnarok.Unification;
using System;

namespace YggdrAshill.Ragnarok
{
    public static class UnificationExtension
    {
        public static void Collect(this ITerminationCollection collection, Action onTerminated)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }
            if (onTerminated == null)
            {
                throw new ArgumentNullException(nameof(onTerminated));
            }

            collection.Collect(new Termination(onTerminated));
        }

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

        public static ITermination Collect(this IExecutionCollection collection, Action onExecuted)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }
            if (onExecuted == null)
            {
                throw new ArgumentNullException(nameof(onExecuted));
            }

            return collection.Collect(new Execution(onExecuted));
        }

        public static ITermination Collect(this IExecution execution, IExecutionCollection collection)
        {
            if (execution == null)
            {
                throw new ArgumentNullException(nameof(execution));
            }
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            return collection.Collect(execution);
        }

        public static IOrigination Collect(this IActivation activation, IExecutionCollection collection)
        {
            if (activation == null)
            {
                throw new ArgumentNullException(nameof(activation));
            }
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            return new Origination(() =>
            {
                return activation.Activate().Collect(collection);
            });
        }
    }
}
