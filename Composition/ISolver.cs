using System;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public interface ISolver
    {
        IActivation CreateActivation(DependencyInjectionRequest request);
        IInfusion CreateFieldInfusion(FieldInjectionRequest request);
        IInfusion CreatePropertyInfusion(PropertyInjectionRequest request);
        IInfusion CreateMethodInfusion(MethodInjectionRequest request);
        IActivation CreateCollectionActivation(Type elementType);
    }
}
