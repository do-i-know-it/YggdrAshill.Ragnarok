using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    internal class FakeAbortion :
        IAbortion
    {
        internal Exception Aborted { get; private set; }

        public void Abort(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            Aborted = exception;
        }
    }
}
