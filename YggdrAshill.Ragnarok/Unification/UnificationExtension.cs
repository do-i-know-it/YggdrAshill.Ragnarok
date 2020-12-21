using YggdrAshill.Ragnarok.Periodization;
using YggdrAshill.Ragnarok.Progression;
using YggdrAshill.Ragnarok.Unification;
using System;

namespace YggdrAshill.Ragnarok
{
    public static class UnificationExtension
    {
        #region ITermination

        public static void Bind(this ITerminationCollection collection, Action onTerminated)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }
            if (onTerminated == null)
            {
                throw new ArgumentNullException(nameof(onTerminated));
            }

            collection.Bind(new Termination(onTerminated));
        }

        public static void Bind(this ITermination termination, ITerminationCollection collection)
        {
            if (termination == null)
            {
                throw new ArgumentNullException(nameof(termination));
            }
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            collection.Bind(termination);
        }

        #endregion

        #region IExecution

        public static ITermination Bind(this IExecutionCollection collection, Action onExecuted)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }
            if (onExecuted == null)
            {
                throw new ArgumentNullException(nameof(onExecuted));
            }

            return collection.Bind(new Execution(onExecuted));
        }

        public static ITermination Bind(this IExecution execution, IExecutionCollection collection)
        {
            if (execution == null)
            {
                throw new ArgumentNullException(nameof(execution));
            }
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            return collection.Bind(execution);
        }

        #endregion

        #region IActivation

        public static IOrigination Bind(this IActivation activation, IExecutionCollection collection)
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
                return activation.Activate().Bind(collection);
            });
        }

        #endregion
    }
}
