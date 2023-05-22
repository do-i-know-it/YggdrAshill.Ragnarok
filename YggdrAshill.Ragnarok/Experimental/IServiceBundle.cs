using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    public interface IServiceBundle<T>
        where T : notnull
    {
        IReadOnlyList<T> Package { get; }
    }
}
