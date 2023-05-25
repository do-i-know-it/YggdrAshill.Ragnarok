using YggdrAshill.Ragnarok.Materialization;
using System;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public interface ISolver
    {
        IActivation CreateActivation(ConstructorInjection injection);
        IInfusion CreateFieldInfusion(FieldInjection injection);
        IInfusion CreatePropertyInfusion(PropertyInjection injection);
        IInfusion CreateMethodInfusion(MethodInjection injection);
        IActivation CreateCollectionActivation(Type elementType);
    }
}
