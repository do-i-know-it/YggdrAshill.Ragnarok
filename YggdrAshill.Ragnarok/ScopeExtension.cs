using YggdrAshill.Ragnarok.Construction;
using System;

namespace YggdrAshill.Ragnarok
{
    public static class ScopeExtension
    {
        public static IScope CreateChildScope(this IScope scope)
        {
            return scope.CreateContext().Build();
        }

        public static IScope CreateChildScope(this IScope scope, IInstallation installation)
        {
            var context = scope.CreateContext();

            installation.Install(context);

            return context.Build();
        }

        public static IScope CreateChildScope(this IScope scope, Action<IContainer> installation)
        {
            return scope.CreateChildScope(new Installation(installation));
        }

        [Obsolete("Use CreateChildScope(IInstallation) instead.")]
        public static IScope CreateScope(this IScope scope, IInstallation installation)
        {
            var context = scope.CreateContext();

            installation.Install(context);

            return context.Build();
        }

        [Obsolete("Use CreateChildScope(Action<IContainer>) instead.")]
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
