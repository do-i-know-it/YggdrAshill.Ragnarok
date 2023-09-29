using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public interface ICompilationV2
    {
        IInstantiationV2 CreateInstantiation(Type type, IReadOnlyList<IParameter> parameterList);
        IInjectionV2 CreateFieldInjection(Type type, IReadOnlyList<IParameter> parameterList);
        IInjectionV2 CreatePropertyInjection(Type type, IReadOnlyList<IParameter> parameterList);
        IInjectionV2 CreateMethodInjection(Type type, IReadOnlyList<IParameter> parameterList);
    }
}
