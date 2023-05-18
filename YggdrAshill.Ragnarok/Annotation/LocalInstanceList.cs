using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    public sealed class LocalInstanceList<T> :
        ILocalInstanceList<T>
    {
        public IReadOnlyList<T> InstanceList { get; }

        [Inject]
        public LocalInstanceList(IReadOnlyList<T> instanceList)
        {
            InstanceList = instanceList;
        }
    }
}
