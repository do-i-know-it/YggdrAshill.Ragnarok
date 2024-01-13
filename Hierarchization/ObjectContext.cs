using System;
using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation of <see cref="IObjectContext"/> using <see cref="IScopedResolver"/> and <see cref="IScopedResolverBuilder"/>.
    /// </summary>
    public sealed class ObjectContext : IObjectContext
    {
        private readonly IScopedResolverBuilder resolverBuilder;

        /// <summary>
        /// Creates <see cref="ObjectContext"/>.
        /// </summary>
        /// <param name="resolverBuilder">
        /// <see cref="IScopedResolverBuilder"/> for <see cref="ObjectContext"/>.
        /// </param>
        public ObjectContext(IScopedResolverBuilder resolverBuilder)
        {
            this.resolverBuilder = resolverBuilder;
        }

        private readonly List<IStatement> statementList = new();
        private readonly List<IOperation> operationList = new();
        private readonly List<IDisposable> disposableList = new();

        /// <inheritdoc/>
        public IObjectResolver Resolver => resolverBuilder;

        /// <inheritdoc/>
        public ICompilation Compilation => resolverBuilder.Compilation;

        /// <inheritdoc/>
        public int Count(IStatementSelection selection)
        {
            return statementList.Where(selection.IsSatisfied).Count();
        }

        /// <inheritdoc/>
        public void Register(IStatement statement)
        {
            if (statementList.Contains(statement))
            {
                return;
            }

            statementList.Add(statement);
        }

        /// <inheritdoc/>
        public void Register(IOperation operation)
        {
            if (operationList.Contains(operation))
            {
                return;
            }

            operationList.Add(operation);
        }

        /// <inheritdoc/>
        public void Register(IDisposable disposable)
        {
            if (disposableList.Contains(disposable))
            {
                return;
            }

            disposableList.Add(disposable);
        }

        /// <inheritdoc/>
        public IObjectContext CreateContext()
        {
            return new ObjectContext(resolverBuilder);
        }

        /// <inheritdoc/>
        public IObjectScope CreateScope()
        {
            var resolver = resolverBuilder.Build(statementList);

            foreach (var disposable in disposableList)
            {
                resolver.Bind(disposable);
            }

            foreach (var operation in operationList)
            {
                operation.Operate(resolver);
            }

            return new ObjectScope(resolver);
        }
    }
}
