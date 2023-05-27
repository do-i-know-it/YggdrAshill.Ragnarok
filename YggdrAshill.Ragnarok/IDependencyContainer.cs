using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Fabrication;
using System;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public interface IDependencyContainer :
        IContainer
    {
        IDependencyInjection RegisterTemporal<T>() where T : notnull;
        IDependencyInjection RegisterLocal<T>() where T : notnull;
        IDependencyInjection RegisterGlobal<T>() where T : notnull;

        IDependencyInjection RegisterTemporal<TInterface, TImplementation>() where TInterface : notnull where TImplementation : TInterface;
        IDependencyInjection RegisterLocal<TInterface, TImplementation>() where TInterface : notnull where TImplementation : TInterface;
        IDependencyInjection RegisterGlobal<TInterface, TImplementation>() where TInterface : notnull where TImplementation : TInterface;

        IInstanceInjection RegisterTemporal<T>(Func<T> instantiation, bool external = true) where T : notnull;
        IInstanceInjection RegisterLocal<T>(Func<T> instantiation, bool external = true) where T : notnull;
        IInstanceInjection RegisterGlobal<T>(Func<T> instantiation, bool external = true) where T : notnull;

        IInstanceInjection RegisterTemporal<TInterface, TImplementation>(Func<TImplementation> instantiation, bool external = true)  where TInterface : notnull where TImplementation : TInterface;
        IInstanceInjection RegisterLocal<TInterface, TImplementation>(Func<TImplementation> instantiation, bool external = true)  where TInterface : notnull where TImplementation : TInterface;
        IInstanceInjection RegisterGlobal<TInterface, TImplementation>(Func<TImplementation> instantiation, bool external = true)  where TInterface : notnull where TImplementation : TInterface;
    }
}
