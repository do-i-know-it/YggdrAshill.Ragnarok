using System;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    // TODO: rename methods.
    /// <summary>
    /// Defines how to select type information to resolve dependencies.
    /// </summary>
    public interface ISelector
    {
        ConstructorInjection CreateConstructorInjection(Type type);
        FieldInjection CreateFieldInjection(Type type);
        PropertyInjection CreatePropertyInjection(Type type);
        MethodInjection CreateMethodInjection(Type type);
        Type GetServiceBundleType(Type elementType);
    }
}
