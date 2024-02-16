using System;
using System.Runtime.CompilerServices;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class SubContainerResolutionExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ISubContainerResolution With(this ISubContainerResolution resolution, Action<IObjectContainer> installation)
        {
            return resolution.With(new Installation(installation));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ISubContainerResolution With<TInstallation>(this ISubContainerResolution resolution)
            where TInstallation : IInstallation
        {
            var installation = resolution.Resolver.Resolve<TInstallation>();

            return resolution.With(installation);
        }
    }
}
