using YggdrAshill.Ragnarok.Periodization;
using System;

namespace YggdrAshill.Ragnarok
{
    public sealed class Termination :
        ITermination
    {
        private readonly Action onTerminated;

        #region Constructor

        public Termination(Action onTerminated)
        {
            if (onTerminated == null)
            {
                throw new ArgumentNullException(nameof(onTerminated));
            }

            this.onTerminated = onTerminated;
        }

        public Termination()
        {
            onTerminated = () =>
            {

            };
        }

        #endregion

        #region ITermination

        public void Terminate()
        {
            onTerminated.Invoke();
        }

        #endregion
    }
}
