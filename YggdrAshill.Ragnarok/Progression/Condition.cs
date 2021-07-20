using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation of <see cref="ICondition"/>.
    /// </summary>
    public sealed class Condition :
        ICondition
    {
        /// <summary>
        /// Executes <see cref="Func{TResult}"/>.
        /// </summary>
        /// <param name="condition">
        /// <see cref="Func{TResult}"/> as <see cref="ICondition.IsSatisfied"/>.
        /// </param>
        /// <returns>
        /// <see cref="Condition"/> created.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="condition"/> is null.
        /// </exception>
        public static Condition Of(Func<bool> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            return new Condition(condition);
        }

        /// <summary>
        /// Returns <see cref="bool"/>.
        /// </summary>
        /// <param name="condition">
        /// <see cref="bool"/> as <see cref="ICondition.IsSatisfied"/>.
        /// </param>
        /// <returns>
        /// <see cref="Condition"/> created.
        /// </returns>
        public static Condition Of(bool condition)
        {
            return new Condition(() => condition);
        }

        /// <summary>
        /// <see cref="Always"/> <see cref="ICondition.IsSatisfied"/>.
        /// </summary>
        public static Condition Always { get; } = Of(true);

        /// <summary>
        /// <see cref="Never"/> <see cref="ICondition.IsSatisfied"/>.
        /// </summary>
        public static Condition Never { get; } = Of(false);

        private readonly Func<bool> condition;

        private Condition(Func<bool> condition)
        {
            this.condition = condition;
        }

        /// <inheritdoc/>
        public bool IsSatisfied => condition.Invoke();
    }
}
