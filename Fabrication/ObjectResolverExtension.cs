using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal static class ObjectResolverExtension
    {
        public static object Resolve(this IObjectResolver resolver, IEnumerable<IParameter> parameterList, Argument argument)
        {
            foreach (var parameter in parameterList)
            {
                if (parameter.Type == argument.Type && parameter.Name == argument.Name)
                {
                    return parameter.Instance;
                }
            }

            return resolver.Resolve(argument.Type);
        }
    }
}
