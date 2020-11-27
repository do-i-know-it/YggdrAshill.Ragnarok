using YggdrAshill.Ragnarok.Administration;
using System;

namespace YggdrAshill.Ragnarok
{
    public sealed class Initiation :
        IInitiation
    {
        private readonly Func<ITermination> onInitiated;

        #region Constructor

        public Initiation(Func<ITermination> onInitiated)
        {
            if (onInitiated == null)
            {
                throw new ArgumentNullException(nameof(onInitiated));
            }

            this.onInitiated = onInitiated;
        }

        public Initiation()
        {
            onInitiated = () =>
            {
                return new Termination();
            };
        }

        #endregion

        #region IInitiation

        public ITermination Initiate()
        {
            return onInitiated.Invoke();
        }

        #endregion
    }
}
