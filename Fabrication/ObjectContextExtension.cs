using System;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class ObjectContextExtension
    {
        public static IObjectContext Install(this IObjectContext context, params IInstallation[] installationList)
        {
            foreach (var installation in installationList)
            {
                installation.Install(context);
            }

            return context;
        }

        public static IObjectContext Install(this IObjectContext context, Action<IObjectContainer> installation)
        {
            return context.Install(new Installation(installation));
        }
    }
}
