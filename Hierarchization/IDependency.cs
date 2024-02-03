using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    public interface IDependency
    {
        IReadOnlyList<Type> DependentTypeList { get; }

        IRealization CreateRealization(IReadOnlyList<IParameter> parameterList);
    }
}
