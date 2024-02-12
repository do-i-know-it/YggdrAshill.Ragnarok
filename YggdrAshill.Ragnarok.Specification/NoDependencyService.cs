using System;

namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class NoDependencyService : IService, IDisposable
    {
        public void Dispose()
        {

        }
    }
}
