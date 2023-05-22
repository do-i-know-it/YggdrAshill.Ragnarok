using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    [Obsolete("Use IServiceBundle<T> instead.")]
    public interface ILocalInstanceList<T>
    {
        IReadOnlyList<T> InstanceList { get; }
    }
}
