using YggdrAshill.Ragnarok.Hierarchization;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Materialization
{
    public interface ICollectionGeneration
    {
        Type ElementType { get; }
        object Create(IScopedResolver resolver, IReadOnlyList<IRegistration> registrationList);
    }
}
