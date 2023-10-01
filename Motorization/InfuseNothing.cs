using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class InfuseNothing : IInfusion
    {
        public static InfuseNothing Instance { get; } = new();

        private InfuseNothing()
        {

        }

        public IReadOnlyList<Argument> ArgumentList { get; } = Array.Empty<Argument>();

        public void Infuse(object instance, object[] parameterList)
        {

        }
    }
}
