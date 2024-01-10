using System;
using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation of <see cref="IScopedResolverBuilder"/>.
    /// </summary>
    public sealed class ScopedResolverBuilder : IScopedResolverBuilder
    {
        private readonly Engine engine;
        private readonly IScopedResolver? parent;

        internal ScopedResolverBuilder(Engine engine, IScopedResolver? parent)
        {
            this.engine = engine;
            this.parent = parent;
        }

        /// <summary>
        /// Creates <see cref="ScopedResolverBuilder"/> for root <see cref="IScopedResolver"/>.
        /// </summary>
        /// <param name="resolver">
        /// <see cref="IRootResolver"/> for <see cref="ScopedResolverBuilder"/>.
        /// </param>
        public ScopedResolverBuilder(IRootResolver resolver) : this(new Engine(resolver), null)
        {

        }

        private readonly List<IStatement> statementList = new()
        {
            ObjectResolverStatement.Instance
        };

        private readonly List<IOperation> operationList = new();
        private readonly List<IDisposable> disposableList = new();

        /// <inheritdoc/>
        public object Resolve(Type type)
        {
            if (typeof(IInstallation).IsAssignableFrom(type))
            {
                return engine.Resolver.Resolve(type);
            }

            if (parent != null)
            {
                return parent.Resolve(type);
            }

            throw new RagnarokNotRegisteredException(type);
        }

        /// <inheritdoc/>
        public ICompilation Compilation => engine;

        /// <inheritdoc/>
        public int StatementCount => statementList.Count;

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
        public IScopedResolverBuilder CreateBuilder()
        {
            return new ScopedResolverBuilder(engine, parent);
        }

        /// <inheritdoc/>
        public IScopedResolver Build()
        {
            using var factory = engine.CreateFactory(statementList);

            var content = factory.Create();

            IScopedResolver resolver = new ScopedResolver(content, engine, parent);

            factory.Validate(resolver);

            foreach (var disposable in disposableList)
            {
                resolver.Bind(disposable);
            }

            foreach (var operation in operationList)
            {
                operation.Operate(resolver);
            }

            return resolver;
        }
    }
}
