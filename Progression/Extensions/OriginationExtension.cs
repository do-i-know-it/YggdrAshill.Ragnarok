using System;

namespace YggdrAshill.Ragnarok.Progression
{
    /// <summary>
    /// Defines extensions for <see cref="IOrigination"/>.
    /// </summary>
    public static class OriginationExtension
    {
        /// <summary>
        /// Binds <see cref="IOrigination"/> to <see cref="ICondition"/>.
        /// </summary>
        /// <param name="origination">
        /// <see cref="IOrigination"/> to bind.
        /// </param>
        /// <param name="condition">
        /// <see cref="ICondition"/> to bind.
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
        [Obsolete]
        public static IOrigination When(this IOrigination origination, ICondition condition)
        {
            if (origination == null)
            {
                throw new ArgumentNullException(nameof(origination));
            }
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            return new OriginateWhenConditionIsSatisfied(condition, origination);
        }
        private sealed class OriginateWhenConditionIsSatisfied :
            IOrigination
        {
            private readonly ICondition condition;

            private readonly IOrigination origination;

            internal OriginateWhenConditionIsSatisfied(ICondition condition, IOrigination origination)
            {
                this.condition = condition;

                this.origination = origination;
            }

            /// <inheritdoc/>
            public void Originate()
            {
                if (!condition.IsSatisfied)
                {
                    return;
                }

                origination.Originate();
            }
        }

        /// <summary>
        /// Binds <see cref="IOrigination"/> to <see cref="IAbortion"/>.
        /// </summary>
        /// <param name="origination">
        /// <see cref="IOrigination"/> to bind.
        /// </param>
        /// <param name="abortion">
        /// <see cref="IAbortion"/> to bind.
        /// </param>
        /// <returns>
        /// <see cref="IOrigination"/> bound.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="origination"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="abortion"/> is null.
        /// </exception>
        [Obsolete]
        public static IOrigination Bind(this IOrigination origination, IAbortion abortion)
        {
            if (origination == null)
            {
                throw new ArgumentNullException(nameof(origination));
            }
            if (abortion == null)
            {
                throw new ArgumentNullException(nameof(abortion));
            }

            return new AbortWhenOriginationHasErrored(origination, abortion);
        }
        private sealed class AbortWhenOriginationHasErrored :
            IOrigination
        {
            private readonly IOrigination origination;

            private readonly IAbortion abortion;

            internal AbortWhenOriginationHasErrored(IOrigination origination, IAbortion abortion)
            {
                this.origination = origination;

                this.abortion = abortion;
            }

            /// <inheritdoc/>
            public void Originate()
            {
                try
                {
                    origination.Originate();
                }
                catch (Exception exception)
                {
                    abortion.Abort(exception);
                }
            }
        }
    }
}
