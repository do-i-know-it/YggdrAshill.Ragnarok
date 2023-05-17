using System;

namespace YggdrAshill.Ragnarok.Materialization
{
    public interface ICodeBuilder
    {
        IActivation CreateActivation(Type type);
        IInfusion CreateFieldInfusion(Type type);
        IInfusion CreatePropertyInfusion(Type type);
        IInfusion CreateMethodInfusion(Type type);
    }
}
