using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ServiceBundle<T> :
        IServiceBundle<T>
        where T : notnull
    {
        public IReadOnlyList<T> Package { get; }

        [Inject]
        public ServiceBundle(IReadOnlyList<T> package)
        {
            Package = package;
        }
    }
}
