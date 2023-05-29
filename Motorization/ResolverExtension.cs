using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Extension for <see cref="IResolver"/>.
    /// </summary>
    public static partial class ResolverExtension
    {
        /// <summary>
        /// Resolve <see cref="Argument"/> from <see cref="IResolver"/> or <see cref="IParameter"/>s.
        /// </summary>
        /// <param name="resolver">
        /// <see cref="IResolver"/> to resolve.
        /// </param>
        /// <param name="parameterList">
        /// <see cref="IParameter"/>s to resolve.
        /// </param>
        /// <param name="argument">
        /// <see cref="Argument"/> to resolve.
        /// </param>
        /// <returns>
        /// <see cref="object"/> resolved.
        /// </returns>
        public static object Resolve(this IResolver resolver, IReadOnlyList<IParameter> parameterList, Argument argument)
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
