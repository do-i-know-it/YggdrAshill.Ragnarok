using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public interface IServiceBundle<T>
        where T : notnull
    {
        IReadOnlyList<T> Package { get; }
    }
}
