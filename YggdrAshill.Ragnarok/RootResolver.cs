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
            var instantiation = activation.ToInstantiate();
            return instantiation.Instantiate(this);
        }
    }
}
