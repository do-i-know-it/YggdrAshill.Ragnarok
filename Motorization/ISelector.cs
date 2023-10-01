using System;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public interface ISelector
    {
        DependencyInjectionRequest RequestDependencyInjection(Type type);
        FieldInjectionRequest RequestFieldInjection(Type type);
        PropertyInjectionRequest RequestPropertyInjection(Type type);
        MethodInjectionRequest RequestMethodInjection(Type type);
        DependencyInjectionRequest RequestServiceBundleInjection(Type elementType);
    }
}
