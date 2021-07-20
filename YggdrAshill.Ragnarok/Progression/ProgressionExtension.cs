using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines extensions for Progression.
    /// </summary>
    public static class ProgressionExtension
    {
        #region ICondition

        /// <summary>
        /// Multiplies <see cref="ICondition"/> and <see cref="Func{TResult}"/>.
        /// </summary>
        /// <param name="first">
        /// <see cref="ICondition"/> to multiply.
        /// </param>
        /// <param name="second">
        /// <see cref="Func{TResult}"/> to multiply.
        /// </param>
        /// <returns>
        /// <see cref="ICondition"/> multiplied.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="first"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="second"/> is null.
        /// </exception>
        public static ICondition And(this ICondition first, Func<bool> second)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }
            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            return first.And(Condition.Of(second));
        }

        /// <summary>
        /// Adds <see cref="ICondition"/> and <see cref="Func{TResult}"/>.
        /// </summary>
        /// <param name="first">
        /// <see cref="ICondition"/> to add.
        /// </param>
        /// <param name="second">
        /// <see cref="Func{TResult}"/> to add.
        /// </param>
        /// <returns>
        /// <see cref="ICondition"/> added.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="first"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="second"/> is null.
        /// </exception>
        public static ICondition Or(this ICondition first, Func<bool> second)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }
            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            return first.Or(Condition.Of(second));
        }

        #endregion

        #region Originate

        /// <summary>
        /// Binds <see cref="IOrigination"/> to <see cref="Func{TResult}"/>.
        /// </summary>
        /// <param name="origination">
        /// <see cref="IOrigination"/> to bind.
        /// </param>
        /// <param name="condition">
        /// <see cref="Func{TResult}"/> to decide if <see cref="IOrigination"/> should be executed.
        /// </param>
        /// <returns>
        /// <see cref="IOrigination"/> bound.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="origination"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="condition"/> is null.
        /// </exception>
        public static IOrigination When(this IOrigination origination, Func<bool> condition)
        {
            if (origination == null)
            {
                throw new ArgumentNullException(nameof(origination));
            }
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            return origination.When(Condition.Of(condition));
        }

        /// <summary>
        /// Binds <see cref="IOrigination"/> to <see cref="IAbortion"/>.
        /// </summary>
        /// <param name="origination">
        /// <see cref="IOrigination"/> to bind.
        /// </param>
        /// <param name="abortion">
        /// <see cref="Action{T}"/> to abort <see cref="Exception"/>.
        /// </param>
        /// <returns>
        /// <see cref="IOrigination"/> bound.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="abortion"/> is null.
        /// </exception>
        public static IOrigination Bind(this IOrigination origination, Action<Exception> abortion)
        {
            if (origination == null)
            {
                throw new ArgumentNullException(nameof(origination));
            }
            if (abortion == null)
            {
                throw new ArgumentNullException(nameof(abortion));
            }

            return origination.Bind(Abortion.Of(abortion));
        }

        /// <summary>
        /// Binds <see cref="IOrigination"/> to <see cref="IAbortion"/>.
        /// </summary>
        /// <param name="origination">
        /// <see cref="IOrigination"/> to bind.
        /// </param>
        /// <param name="abortion">
        /// <see cref="Action"/> to execute when this has aborted.
        /// </param>
        /// <returns>
        /// <see cref="IOrigination"/> bound.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="abortion"/> is null.
        /// </exception>
        public static IOrigination Bind(this IOrigination origination, Action abortion)
        {
            if (origination == null)
            {
                throw new ArgumentNullException(nameof(origination));
            }
            if (abortion == null)
            {
                throw new ArgumentNullException(nameof(abortion));
            }

            return origination.Bind(Abortion.Of(abortion));
        }

        /// <summary>
        /// Binds <see cref="IOrigination"/> to <see cref="CompositeOrigination"/>.
        /// </summary>
        /// <param name="origination">
        /// <see cref="IOrigination"/> to bind.
        /// </param>
        /// <param name="composite">
        /// <see cref="CompositeOrigination"/> to bind.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="origination"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="composite"/> is null.
        /// </exception>
        public static void Bind(this IOrigination origination, CompositeOrigination composite)
        {
            if (origination == null)
            {
                throw new ArgumentNullException(nameof(origination));
            }
            if (composite == null)
            {
                throw new ArgumentNullException(nameof(composite));
            }

            composite.Bind(origination);
        }

        #endregion

        #region Terminate

        /// <summary>
        /// Binds <see cref="ITermination"/> to <see cref="Func{TResult}"/>.
        /// </summary>
        /// <param name="termination">
        /// <see cref="ITermination"/> to bind.
        /// </param>
        /// <param name="condition">
        /// <see cref="Func{TResult}"/> to decide if <see cref="ITermination"/> should be executed.
        /// </param>
        /// <returns>
        /// <see cref="ITermination"/> bound.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="termination"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="condition"/> is null.
        /// </exception>
        public static ITermination When(this ITermination termination, Func<bool> condition)
        {
            if (termination == null)
            {
                throw new ArgumentNullException(nameof(termination));
            }
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            return termination.When(Condition.Of(condition));
        }

        /// <summary>
        /// Binds <see cref="ITermination"/> to <see cref="IAbortion"/>.
        /// </summary>
        /// <param name="termination">
        /// <see cref="ITermination"/> to bind.
        /// </param>
        /// <param name="abortion">
        /// <see cref="Action{T}"/> to abort <see cref="Exception"/>.
        /// </param>
        /// <returns>
        /// <see cref="ITermination"/> bound.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="abortion"/> is null.
        /// </exception>
        public static ITermination Bind(this ITermination termination, Action<Exception> abortion)
        {
            if (termination == null)
            {
                throw new ArgumentNullException(nameof(termination));
            }
            if (abortion == null)
            {
                throw new ArgumentNullException(nameof(abortion));
            }

            return termination.Bind(Abortion.Of(abortion));
        }

        /// <summary>
        /// Binds <see cref="ITermination"/> to <see cref="IAbortion"/>.
        /// </summary>
        /// <param name="termination">
        /// <see cref="ITermination"/> to bind.
        /// </param>
        /// <param name="abortion">
        /// <see cref="Action"/> to execute when this has aborted.
        /// </param>
        /// <returns>
        /// <see cref="ITermination"/> bound.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="abortion"/> is null.
        /// </exception>
        public static ITermination Bind(this ITermination termination, Action abortion)
        {
            if (termination == null)
            {
                throw new ArgumentNullException(nameof(termination));
            }
            if (abortion == null)
            {
                throw new ArgumentNullException(nameof(abortion));
            }

            return termination.Bind(Abortion.Of(abortion));
        }

        /// <summary>
        /// Binds <see cref="ITermination"/> to <see cref="CompositeTermination"/>.
        /// </summary>
        /// <param name="termination">
        /// <see cref="ITermination"/> to bind.
        /// </param>
        /// <param name="composite">
        /// <see cref="CompositeTermination"/> to bind.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="termination"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="composite"/> is null.
        /// </exception>
        public static void Bind(this ITermination termination, CompositeTermination composite)
        {
            if (termination == null)
            {
                throw new ArgumentNullException(nameof(termination));
            }
            if (composite == null)
            {
                throw new ArgumentNullException(nameof(composite));
            }

            composite.Bind(termination);
        }

        /// <summary>
        /// Converts <see cref="ITermination"/> into <see cref="IDisposable"/>.
        /// </summary>
        /// <param name="termination">
        /// <see cref="ITermination"/>.
        /// </param>
        /// <returns>
        /// <see cref="IDisposable"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="termination"/> is null.
        /// </exception>
        public static IDisposable ToDisposable(this ITermination termination)
        {
            if (termination == null)
            {
                throw new ArgumentNullException(nameof(termination));
            }

            return new Disposable(termination);
        }
        private sealed class Disposable :
            IDisposable
        {
            private readonly ITermination termination;

            private bool disposed;

            internal Disposable(ITermination termination)
            {
                this.termination = termination;
            }

            public void Dispose()
            {
                if (disposed)
                {
                    throw new ObjectDisposedException(nameof(ITermination));
                }

                termination.Terminate();

                disposed = true;
            }
        }

        #endregion

        #region Execute

        /// <summary>
        /// Binds <see cref="IExecution"/> to <see cref="Func{TResult}"/>.
        /// </summary>
        /// <param name="execution">
        /// <see cref="IExecution"/> to bind.
        /// </param>
        /// <param name="condition">
        /// <see cref="Func{TResult}"/> to decide if <see cref="IExecution"/> should be executed.
        /// </param>
        /// <returns>
        /// <see cref="IExecution"/> bound.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="execution"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="condition"/> is null.
        /// </exception>
        public static IExecution When(this IExecution execution, Func<bool> condition)
        {
            if (execution == null)
            {
                throw new ArgumentNullException(nameof(execution));
            }
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            return execution.When(Condition.Of(condition));
        }

        /// <summary>
        /// Binds <see cref="IExecution"/> to <see cref="IAbortion"/>.
        /// </summary>
        /// <param name="execution">
        /// <see cref="IExecution"/> to bind.
        /// </param>
        /// <param name="abortion">
        /// <see cref="Action{T}"/> to abort <see cref="Exception"/>.
        /// </param>
        /// <returns>
        /// <see cref="IExecution"/> bound.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="abortion"/> is null.
        /// </exception>
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

            return execution.Bind(Abortion.Of(abortion));
        }

        /// <summary>
        /// Binds <see cref="IExecution"/> to <see cref="IAbortion"/>.
        /// </summary>
        /// <param name="execution">
        /// <see cref="IExecution"/> to bind.
        /// </param>
        /// <param name="abortion">
        /// <see cref="Action"/> to execute when this has aborted.
        /// </param>
        /// <returns>
        /// <see cref="IExecution"/> bound.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="abortion"/> is null.
        /// </exception>
        public static IExecution Bind(this IExecution execution, Action abortion)
        {
            if (execution == null)
            {
                throw new ArgumentNullException(nameof(execution));
            }
            if (abortion == null)
            {
                throw new ArgumentNullException(nameof(abortion));
            }

            return execution.Bind(Abortion.Of(abortion));
        }

        /// <summary>
        /// Binds <see cref="IExecution"/> to <see cref="CompositeExecution"/>.
        /// </summary>
        /// <param name="execution">
        /// <see cref="IExecution"/> to bind.
        /// </param>
        /// <param name="composite">
        /// <see cref="CompositeExecution"/> to bind.
        /// </param>
        /// <returns>
        /// <see cref="ITermination"/> to stop binding <paramref name="execution"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="execution"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="composite"/> is null.
        /// </exception>
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

        #endregion

        #region Abort

        /// <summary>
        /// Binds <see cref="IAbortion"/> to <see cref="CompositeAbortion"/>.
        /// </summary>
        /// <param name="abortion">
        /// <see cref="IAbortion"/> to bind.
        /// </param>
        /// <param name="composite">
        /// <see cref="CompositeAbortion"/> to bind.
        /// </param>
        /// <returns>
        /// <see cref="ITermination"/> to stop binding <paramref name="abortion"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="abortion"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="composite"/> is null.
        /// </exception>
        public static ITermination Bind(this IAbortion abortion, CompositeAbortion composite)
        {
            if (abortion == null)
            {
                throw new ArgumentNullException(nameof(abortion));
            }
            if (composite == null)
            {
                throw new ArgumentNullException(nameof(composite));
            }

            return composite.Bind(abortion);
        }

        #endregion
    }
}
