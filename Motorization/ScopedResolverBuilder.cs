using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation of <see cref="IScopedResolverContext"/>.
    /// </summary>
    public sealed class ScopedResolverBuilder : IScopedResolverBuilder
    {
        private readonly Compilation compilation;
        private readonly IScopedResolverV2? parent;

        /// <summary>
        /// Creates <see cref="ScopedResolverBuilder"/> for child <see cref="IScopedResolverV2"/>.
        /// </summary>
        /// <param name="compilation">
        /// <see cref="Compilation"/> for <see cref="ScopedResolverBuilder"/>.
        /// </param>
        /// <param name="parent">
        /// <see cref="IScopedResolverV2"/> for <see cref="ScopedResolverBuilder"/>.
        /// </param>
        public ScopedResolverBuilder(Compilation compilation, IScopedResolverV2? parent)
        {
            this.compilation = compilation;
            this.parent = parent;
        }

        /// <summary>
        /// Creates <see cref="ScopedResolverBuilder"/> for root <see cref="IScopedResolverV2"/>.
        /// </summary>
        /// <param name="selector">
        /// <see cref="ISelector"/> for <see cref="ScopedResolverBuilder"/>.
        /// </param>
        /// <param name="solver">
        /// <see cref="ISolver"/> for <see cref="ScopedResolverBuilder"/>.
        /// </param>
        public ScopedResolverBuilder(ISelector selector, ISolver solver)
            : this(new Compilation(selector, solver), null)
        {

        }

        private readonly List<IDescriptionV2> descriptionList = new List<IDescriptionV2>()
        {
            ObjectResolverDescription.Instance
        };

        private readonly List<IOperation> operationList = new List<IOperation>();
        private readonly List<IDisposable> disposableList = new List<IDisposable>();

        /// <inheritdoc/>
        public ICompilationV2 Compilation => compilation;

        /// <inheritdoc/>
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        /// <inheritdoc/>
        public object Resolve(Type type)
        {
            if (parent != null)
            {
                return parent.Resolve(type);
            }

            var activation = Compilation.GetActivation(type);

            var argumentList = activation.ArgumentList;

            // TODO: object pooling.
            var instanceList = new object[argumentList.Count];

            for (var index = 0; index < argumentList.Count; index++)
            {
                var argument = argumentList[index];

                instanceList[index] = Resolve(argument.Type);
            }

            return activation.Activate(instanceList);
        }

        /// <inheritdoc/>
        public void Register(IDescriptionV2 description)
        {
            if (descriptionList.Contains(description))
            {
                return;
            }

            descriptionList.Add(description);
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
            return new ScopedResolverBuilder(compilation, parent);
        }

        /// <inheritdoc/>
        public IScopedResolverV2 Build()
        {
            using var factory = new DictionaryFactory(compilation);

            var content = factory.CreateContent(descriptionList);

            var resolver = new ScopedResolverV2(content, compilation, parent);

            TypeAnalysis.Validate(descriptionList, resolver);

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
