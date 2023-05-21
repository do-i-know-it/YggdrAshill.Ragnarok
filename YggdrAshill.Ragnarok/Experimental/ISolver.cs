using YggdrAshill.Ragnarok.Materialization;
using System;

namespace YggdrAshill.Ragnarok
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
        [Obsolete("Use ISelector.GetServiceBundleType(Type) instead.")]
        Type GetLocalInstanceListType(Type elementType);
    }
}
