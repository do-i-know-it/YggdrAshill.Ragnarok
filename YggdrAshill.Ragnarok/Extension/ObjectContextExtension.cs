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
            context.Install(installationList);

            return context.CreateScope();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IObjectScope CreateCurrentScope(this IObjectContext context, Action<IObjectContainer> installation)
        {
            context.Install(installation);

            return context.CreateScope();
        }
    }
}
