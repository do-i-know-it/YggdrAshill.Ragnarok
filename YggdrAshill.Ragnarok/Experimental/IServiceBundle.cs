using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Memorization
{
    // TODO: add document comments.
    public interface IServiceBundle<T>
        where T : notnull
    {
        IReadOnlyList<T> Package { get; }
    }
}
