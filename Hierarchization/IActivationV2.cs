using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to inject dependencies into instance.
    /// </summary>
    public interface IInfusionV2
    {
        /// <summary>
        /// <see cref="IDependency"/>s to instantiate.
        /// </summary>
        IDependency Dependency { get; }

        /// <summary>
        /// Injects <see cref="object"/>s into instance.
        /// </summary>
        /// <param name="instance">
        /// <see cref="object"/> to inject dependencies into.
        /// </param>
        /// <param name="parameterList">
        /// <see cref="object"/>s to inject instance.
        /// </param>
        void Infuse(object instance, object[] parameterList);
    }
    /// <summary>
    /// Defines how to create instance with dependencies.
    /// </summary>
    public interface IActivationV2
    {
        /// <summary>
        /// <see cref="IDependency"/>s to instantiate.
        /// </summary>
        IDependency Dependency { get; }

        /// <summary>
        /// Instantiates with <see cref="object"/>s.
        /// </summary>
        /// <param name="parameterList">
        /// <see cref="object"/>s to instantiate.
        /// </param>
        /// <returns>
        /// <see cref="object"/> instantiated.
        /// </returns>
        object Activate(object[] parameterList);
    }
}
