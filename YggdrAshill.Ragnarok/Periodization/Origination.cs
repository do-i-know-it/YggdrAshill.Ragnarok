using YggdrAshill.Ragnarok.Periodization;
using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation of <see cref="IOrigination"/>.
    /// </summary>
    public sealed class Origination :
        IOrigination
    {
        private readonly Func<ITermination> onOriginated;

        #region Constructor

        /// <summary>
        /// Constructs an instance.
        /// </summary>
        /// <param name="onOriginated">
        /// <see cref="Func{ITermination}"/> executed when this has originated.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="onOriginated"/> is null.
        /// </exception>
        public Origination(Func<ITermination> onOriginated)
        {
            if (onOriginated == null)
            {
                throw new ArgumentNullException(nameof(onOriginated));
            }

            this.onOriginated = onOriginated;
        }

        /// <summary>
        /// Constructs an instance to do nothing when this has originated.
        /// </summary>
        public Origination()
        {
            onOriginated = () =>
            {
                return new Termination();
            };
        }

        #endregion

        #region IOrigination

        /// <inheritdoc/>
        public ITermination Originate()
        {
            return onOriginated.Invoke();
        }

        #endregion
    }
}
