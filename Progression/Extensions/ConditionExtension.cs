using System;

namespace YggdrAshill.Ragnarok.Progression
{
    /// <summary>
    /// Defines extensions for <see cref="ICondition"/>.
    /// </summary>
    public static class ConditionExtension
    {
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
    }
}
