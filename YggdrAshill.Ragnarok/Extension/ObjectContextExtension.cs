using System;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class ObjectContextExtension
    {
        public static IObjectScope CreateCurrentScope(this IObjectContext context, params IInstallation[] installationList)
        {
            context.Install(installationList);

            return context.CreateScope();
        }

        public static IObjectScope CreateCurrentScope(this IObjectContext context, Action<IObjectContainer> installation)
        {
            context.Install(installation);

            return context.CreateScope();
        }
    }
}
