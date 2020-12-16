using System;

namespace YggdrAshill.Ragnarok
{
    public sealed class Abortion :
        IAbortion
    {
        private readonly Action<Exception> onAborted;

        #region Constructor

        public Abortion(Action<Exception> onAborted)
        {
            if (onAborted == null)
            {
                throw new ArgumentNullException(nameof(onAborted));
            }

            this.onAborted = onAborted;
        }

        public Abortion()
        {
            onAborted = (_) =>
            {

            };
        }

        #endregion

        #region IAbortion

        public void Abort(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            onAborted.Invoke(exception);
        }

        #endregion
    }
}
