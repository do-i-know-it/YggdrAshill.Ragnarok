using System;

namespace YggdrAshill.Ragnarok.Materialization
{
    public interface ISolver
    {
        IActivation CreateActivation(ConstructorInjection injection);
        IInfusion CreateFieldInfusion(FieldInjection injection);
        IInfusion CreatePropertyInfusion(PropertyInjection injection);
        IInfusion CreateMethodInfusion(MethodInjection injection);

        [Obsolete("Use CreateCollectionActivation(Type) instead.")]
        ICollectionGeneration CreateCollectionGeneration(Type type);
        IActivation CreateCollectionActivation(Type elementType);
    }
}
