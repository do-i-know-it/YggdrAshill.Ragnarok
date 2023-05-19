using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    public interface ILocalInstanceList<T>
    {
        IReadOnlyList<T> InstanceList { get; }
    }
}
