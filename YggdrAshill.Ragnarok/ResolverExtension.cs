using YggdrAshill.Ragnarok.Construction;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    public static class ResolverExtension
    {
        public static T Resolve<T>(this IResolver resolver)
        {
            return (T)resolver.Resolve(typeof(T));
        }

        [Obsolete]
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
