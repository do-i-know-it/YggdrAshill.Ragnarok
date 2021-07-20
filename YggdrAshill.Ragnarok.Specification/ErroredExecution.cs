using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    internal class ErroredExecution :
        IExecution
    {
        internal ErroredExecution(Exception expected)
        {
            Expected = expected;
        }

        internal Exception Expected { get; }

        public void Execute()
        {
            throw Expected;
        }
    }
}
