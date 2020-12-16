using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok
{
    public sealed class Activation :
        IActivation
    {
        private readonly Func<IExecution> onActivated;

        #region Constructor

        public Activation(Func<IExecution> onActivated)
        {
            if (onActivated == null)
            {
                throw new ArgumentNullException(nameof(onActivated));
            }

            this.onActivated = onActivated;
        }

        public Activation(Action onExecuted)
        {
            if (onExecuted == null)
            {
                throw new ArgumentNullException(nameof(onExecuted));
            }

            onActivated = () =>
            {
                return new Execution(() =>
                {
                    onExecuted.Invoke();
                });
            };
        }

        public Activation()
        {
            onActivated = () =>
            {
                return new Execution();
            };
        }

        #endregion

        #region IActivation

        public IExecution Activate()
        {
            return onActivated.Invoke();
        }

        #endregion
    }
}
