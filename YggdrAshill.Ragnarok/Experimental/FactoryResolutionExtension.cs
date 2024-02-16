using System;
using System.Runtime.CompilerServices;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class FactoryResolutionExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IFactoryResolution With(this IFactoryResolution resolution, Action<IObjectContainer> installation)
        {
            return resolution.With(new Installation(installation));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IFactoryResolution With<TInstallation>(this IFactoryResolution resolution)
            where TInstallation : IInstallation
        {
            var installation = resolution.Resolver.Resolve<TInstallation>();

            return resolution.With(installation);
        }
    }
}
