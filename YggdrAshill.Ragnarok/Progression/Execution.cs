using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok
{
    public sealed class Execution :
        IExecution
    {
        private readonly Action onExecuted;

        #region Constructor

        public Execution(Action onExecuted)
        {
            if (onExecuted == null)
            {
                throw new ArgumentNullException(nameof(onExecuted));
            }

            this.onExecuted = onExecuted;
        }

        public Execution()
        {
            onExecuted = () =>
            {

            };
        }

        #endregion

        #region IExecution

        public void Execute()
        {
            onExecuted.Invoke();
        }

        #endregion
    }
}
