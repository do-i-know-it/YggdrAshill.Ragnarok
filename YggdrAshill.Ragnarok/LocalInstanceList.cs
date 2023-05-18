using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    public sealed class LocalInstanceList<T> :
        ILocalInstanceList<T>
    {
        private readonly T[] instanceList;
        public IReadOnlyList<T> InstanceList => instanceList;

        [Inject]
        public LocalInstanceList(T[] instanceList)
        {
            this.instanceList = instanceList;
        }
    }
}
