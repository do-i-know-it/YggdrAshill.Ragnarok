using YggdrAshill.Ragnarok.Construction;
using System;

namespace YggdrAshill.Ragnarok
{
    public static class ScopeExtension
    {
        public static IScope CreateScope(this IScope scope, IInstallation installation)
        {
            var context = scope.CreateContext();

            installation.Install(context);

            return context.Build();
        }

        public static IScope CreateScope(this IScope scope, Action<IContainer>? installation = null)
        {
            if (installation == null)
            {
                return scope.CreateContext().Build();
            }

            return scope.CreateScope(new Installation(installation));
        }
    }
}
