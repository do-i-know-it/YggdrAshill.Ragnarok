using System;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class ObjectContextExtension
    {
        public static IObjectScope CreateCurrentScope(this IObjectContext context, params IInstallation[] installationList)
        {
            foreach (var installation in installationList)
            {
                installation.Install(context);
            }

            return context.CreateScope();
        }

        public static IObjectScope CreateCurrentScope(this IObjectContext context, Action<IObjectContainer> installation)
        {
            return context.CreateCurrentScope(new Installation(installation));
        }
    }
}
