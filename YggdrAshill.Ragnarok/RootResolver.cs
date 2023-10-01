using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Default implementation of <see cref="IRootResolver"/>.
    /// </summary>
    public sealed class RootResolver : IRootResolver
    {
        /// <inheritdoc/>
        public ISelector Selector { get; }

        /// <inheritdoc/>
        public ISolver Solver { get; }

        public RootResolver(ISelector selector, ISolver solver)
        {
            Selector = selector;
            Solver = solver;
        }

        /// <inheritdoc/>
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        /// <inheritdoc/>
        public object Resolve(Type type)
        {
            var request = Selector.RequestDependencyInjection(type);
            var activation = Solver.CreateActivation(request);

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
    }
}
