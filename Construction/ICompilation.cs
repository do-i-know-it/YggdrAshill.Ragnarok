using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Construction
{
    public interface ICompilation
    {
        IInstantiation GetInstantiation(Type type, IReadOnlyList<IParameter> parameterList);
        IInjection GetFieldInjection(Type type, IReadOnlyList<IParameter> parameterList);
        IInjection GetPropertyInjection(Type type, IReadOnlyList<IParameter> parameterList);
        IInjection GetMethodInjection(Type type, IReadOnlyList<IParameter> parameterList);
    }
}
