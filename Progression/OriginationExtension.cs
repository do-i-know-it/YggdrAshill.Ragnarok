using System;

namespace YggdrAshill.Ragnarok.Progression
{
    /// <summary>
    /// Defines extensions for <see cref="IOrigination"/>.
    /// </summary>
    public static class OriginationExtension
    {
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
        /// <see cref="IOrigination"/> bounded.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="abortion"/> is null.
        /// </exception>
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

            return new Origination(origination, abortion);
        }
        private sealed class Origination :
            IOrigination
        {
            private readonly IOrigination origination;

            private readonly IAbortion abortion;

            internal Origination(IOrigination origination, IAbortion abortion)
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
