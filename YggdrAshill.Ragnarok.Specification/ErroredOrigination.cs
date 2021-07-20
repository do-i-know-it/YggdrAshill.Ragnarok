using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    internal class ErroredOrigination :
        IOrigination
    {
        internal ErroredOrigination(Exception expected)
        {
            Expected = expected;
        }

        internal Exception Expected { get; }

        public void Originate()
        {
            throw Expected;
        }
    }
}
