using System;
using System.Runtime.CompilerServices;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class ObjectContextExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IObjectScope CreateCurrentScope(this IObjectContext context, params IInstallation[] installationList)
        {
            InstallationList.Install(context, installationList);

            return context.CreateScope();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IObjectScope CreateCurrentScope(this IObjectContext context, Action<IObjectContainer> installation)
        {
            installation.Invoke(context);

            return context.CreateScope();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IObjectScope CreateCurrentScope<TInstallation>(this IObjectContext context)
            where TInstallation : IInstallation
        {
            var installation = context.Resolver.Resolve<TInstallation>();

            installation.Install(context);

            return context.CreateScope();
        }
    }
}
