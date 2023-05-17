using System;
using System.Collections.Generic;
using YggdrAshill.Ragnarok.Construction;

namespace YggdrAshill.Ragnarok.Materialization
{
    public static class ResolverExtension
    {
        public static object Resolve(this IResolver resolver, IReadOnlyList<IParameter> parameterList, Type parameterType, string parameterName)
        {
            foreach (var parameter in parameterList)
            {
                if (parameter.Type == parameterType && parameter.Name == parameterName)
                {
                    return parameter.Instance;
                }
            }

            return resolver.Resolve(parameterType);
        }
    }
}
