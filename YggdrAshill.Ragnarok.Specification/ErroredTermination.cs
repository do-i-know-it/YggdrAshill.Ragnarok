using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    internal class ErroredTermination :
        ITermination
    {
        internal ErroredTermination(Exception expected)
        {
            Expected = expected;
        }

        internal Exception Expected { get; }

        public void Terminate()
        {
            throw Expected;
        }
    }
}
