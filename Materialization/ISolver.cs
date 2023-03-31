using System;

namespace YggdrAshill.Ragnarok.Materialization
{
    public interface ISolver
    {
        IActivation CreateActivation(ConstructorInjection injection);
        IInfusion CreateFieldInfusion(FieldInjection injection);
        IInfusion CreatePropertyInfusion(PropertyInjection injection);
        IInfusion CreateMethodInfusion(MethodInjection injection);
        ICollectionGeneration CreateCollectionGeneration(Type type);
    }
}
