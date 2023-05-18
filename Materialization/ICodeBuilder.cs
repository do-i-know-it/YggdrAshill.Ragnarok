using System;

namespace YggdrAshill.Ragnarok.Materialization
{
    public interface ICodeBuilder
    {
        IActivation GetActivation(Type type);
        IInfusion GetFieldInfusion(Type type);
        IInfusion GetPropertyInfusion(Type type);
        IInfusion GetMethodInfusion(Type type);
    }
}
