using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class ObjectResolverExtension
    {
        public static object Resolve(this IObjectResolver resolver, IEnumerable<IParameter> parameterList, Argument argument)
        {
            foreach (var parameter in parameterList)
            {
                if (parameter.CanResolve(argument, out var instance))
                {
                    return instance;
                }
            }

            return resolver.Resolve(argument.Type);
        }
    }
}