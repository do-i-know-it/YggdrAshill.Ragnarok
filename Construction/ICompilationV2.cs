using System;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public interface ICompilationV2
    {
        IActivation GetActivation(Type type);
        IInfusion GetFieldInfusion(Type type);
        IInfusion GetPropertyInfusion(Type type);
        IInfusion GetMethodInfusion(Type type);
    }
}
