using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to build <see cref="IScopedResolver"/>.
    /// </summary>
    public interface IScopedResolverContext : IObjectResolver
    {
        /// <summary>
        /// <see cref="Interpretation"/> to resolve dependency.
        /// </summary>
        IInterpretation Interpretation { get; }

        /// <summary>
        /// Creates a <see cref="IScopedResolver"/>.
        /// </summary>
        /// <param name="statementList">
        /// <see cref="IStatement"/>s to build.
        /// </param>
        /// <returns>
        /// <see cref="IScopedResolver"/> created.
        /// </returns>
        IScopedResolver Build(IReadOnlyList<IStatement> statementList);
    }
}
