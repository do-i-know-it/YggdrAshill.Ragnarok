using YggdrAshill.Ragnarok.Administration;
using System;

namespace YggdrAshill.Ragnarok
{
    public sealed class Origination :
        IOrigination
    {
        private readonly Func<ITermination> onOriginated;

        #region Constructor

        public Origination(Func<ITermination> onOriginated)
        {
            if (onOriginated == null)
            {
                throw new ArgumentNullException(nameof(onOriginated));
            }

            this.onOriginated = onOriginated;
        }

        public Origination()
        {
            onOriginated = () =>
            {
                return new Termination();
            };
        }

        #endregion

        #region IOrigination

        public ITermination Originate()
        {
            return onOriginated.Invoke();
        }

        #endregion
    }
}
