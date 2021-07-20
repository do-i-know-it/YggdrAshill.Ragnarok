using System;

namespace YggdrAshill.Ragnarok.Progression
{
    /// <summary>
    /// Defines extensions for <see cref="ICondition"/>.
    /// </summary>
    public static class ConditionExtension
    {

        #region Originate

        /// <summary>
        /// Combine <see cref="IOrigination"/> and <see cref="IOrigination"/>.
        /// </summary>
        /// <param name="condition">
        /// <see cref="ICondition"/> to decide which <see cref="IOrigination"/> should be executed.
        /// </param>
        /// <param name="satisfied">
        /// <see cref="IOrigination"/> to combine.
        /// </param>
        /// <param name="unsatisfied">
        /// <see cref="IOrigination"/> to combine.
        /// </param>
        /// <returns>
        /// <see cref="IOrigination"/> combined.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="condition"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="satisfied"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="unsatisfied"/> is null.
        /// </exception>
        public static IOrigination Combine(this ICondition condition, IOrigination satisfied, IOrigination unsatisfied)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }
            if (satisfied == null)
            {
                throw new ArgumentNullException(nameof(satisfied));
            }
            if (unsatisfied == null)
            {
                throw new ArgumentNullException(nameof(unsatisfied));
            }

            return new Origination(condition, satisfied, unsatisfied);
        }
        private sealed class Origination :
            IOrigination
        {
            private readonly ICondition condition;

            private readonly IOrigination satisfied;

            private readonly IOrigination unsatisfied;

            public Origination(ICondition condition, IOrigination satisfied, IOrigination unsatisfied)
            {
                this.condition = condition;

                this.satisfied = satisfied;

                this.unsatisfied = unsatisfied;
            }

            /// <inheritdoc/>
            public void Originate()
            {
                if (condition.IsSatisfied)
                {
                    satisfied.Originate();
                }
                else
                {
                    unsatisfied.Originate();
                }
            }
        }

        #endregion

        #region Terminate

        /// <summary>
        /// Combine <see cref="ITermination"/> and <see cref="ITermination"/>.
        /// </summary>
        /// <param name="condition">
        /// <see cref="ICondition"/> to decide which <see cref="ITermination"/> should be executed.
        /// </param>
        /// <param name="satisfied">
        /// <see cref="ITermination"/> to combine.
        /// </param>
        /// <param name="unsatisfied">
        /// <see cref="ITermination"/> to combine.
        /// </param>
        /// <returns>
        /// <see cref="ITermination"/> combined.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="condition"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="satisfied"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="unsatisfied"/> is null.
        /// </exception>
        public static ITermination Combine(this ICondition condition, ITermination satisfied, ITermination unsatisfied)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }
            if (satisfied == null)
            {
                throw new ArgumentNullException(nameof(satisfied));
            }
            if (unsatisfied == null)
            {
                throw new ArgumentNullException(nameof(unsatisfied));
            }

            return new Termination(condition, satisfied, unsatisfied);
        }
        private sealed class Termination :
            ITermination
        {
            private readonly ICondition condition;

            private readonly ITermination satisfied;

            private readonly ITermination unsatisfied;

            public Termination(ICondition condition, ITermination satisfied, ITermination unsatisfied)
            {
                this.condition = condition;

                this.satisfied = satisfied;

                this.unsatisfied = unsatisfied;
            }

            /// <inheritdoc/>
            public void Terminate()
            {
                if (condition.IsSatisfied)
                {
                    satisfied.Terminate();
                }
                else
                {
                    unsatisfied.Terminate();
                }
            }
        }

        #endregion

        #region Execute

        /// <summary>
        /// Combine <see cref="IExecution"/> and <see cref="IExecution"/>.
        /// </summary>
        /// <param name="condition">
        /// <see cref="ICondition"/> to decide which <see cref="IExecution"/> should be executed.
        /// </param>
        /// <param name="satisfied">
        /// <see cref="IExecution"/> to combine.
        /// </param>
        /// <param name="unsatisfied">
        /// <see cref="IExecution"/> to combine.
        /// </param>
        /// <returns>
        /// <see cref="IExecution"/> combined.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="condition"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="satisfied"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="unsatisfied"/> is null.
        /// </exception>
        public static IExecution Combine(this ICondition condition, IExecution satisfied, IExecution unsatisfied)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }
            if (satisfied == null)
            {
                throw new ArgumentNullException(nameof(satisfied));
            }
            if (unsatisfied == null)
            {
                throw new ArgumentNullException(nameof(unsatisfied));
            }

            return new Execution(condition, satisfied, unsatisfied);
        }
        private sealed class Execution :
            IExecution
        {
            private readonly ICondition condition;

            private readonly IExecution satisfied;

            private readonly IExecution unsatisfied;

            public Execution(ICondition condition, IExecution satisfied, IExecution unsatisfied)
            {
                this.condition = condition;

                this.satisfied = satisfied;

                this.unsatisfied = unsatisfied;
            }

            /// <inheritdoc/>
            public void Execute()
            {
                if (condition.IsSatisfied)
                {
                    satisfied.Execute();
                }
                else
                {
                    unsatisfied.Execute();
                }
            }
        }

        #endregion

        #region ICondition

        /// <summary>
        /// Inverts <see cref="ICondition"/>.
        /// </summary>
        /// <param name="condition">
        /// <see cref="ICondition"/> to invert.
        /// </param>
        /// <returns>
        /// <see cref="ICondition"/> inverted.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="condition"/> is null.
        /// </exception>
        public static ICondition Not(this ICondition condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            return new Invert(condition);
        }
        private sealed class Invert :
            ICondition
        {
            private readonly ICondition condition;

            internal Invert(ICondition condition)
            {
                this.condition = condition;
            }

            /// <inheritdoc/>
            public bool IsSatisfied => !condition.IsSatisfied;
        }

        /// <summary>
        /// Multiplies one <see cref="ICondition"/> and another <see cref="ICondition"/>.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns>
        /// <see cref="ICondition"/> multiplied.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="first"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="second"/> is null.
        /// </exception>
        public static ICondition And(this ICondition first, ICondition second)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }
            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            return new Multiply(first, second);
        }
        private sealed class Multiply :
            ICondition
        {
            private readonly ICondition first;

            private readonly ICondition second;

            internal Multiply(ICondition first, ICondition second)
            {
                this.first = first;

                this.second = second;
            }

            /// <inheritdoc/>
            public bool IsSatisfied
                => first.IsSatisfied
                && second.IsSatisfied;
        }

        /// <summary>
        /// Adds one <see cref="ICondition"/> and another <see cref="ICondition"/>.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns>
        /// <see cref="ICondition"/> added.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="first"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="second"/> is null.
        /// </exception>
        public static ICondition Or(this ICondition first, ICondition second)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }
            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            return new Add(first, second);
        }
        private sealed class Add :
            ICondition
        {
            private readonly ICondition first;

            private readonly ICondition second;

            internal Add(ICondition first, ICondition second)
            {
                this.first = first;

                this.second = second;
            }

            /// <inheritdoc/>
            public bool IsSatisfied
                => first.IsSatisfied
                || second.IsSatisfied;
        }

        #endregion
    }
}
