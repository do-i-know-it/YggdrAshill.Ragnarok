using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to decide dependencies.
    /// </summary>
    public interface IDecision
    {
        /// <summary>
        /// Gets <see cref="ConstructorInjectionRequest"/> for <see cref="Type"/>.
        /// </summary>
        /// <param name="type">
        /// <see cref="Type"/> for request.
        /// </param>
        /// <returns>
        /// <see cref="ConstructorInjectionRequest"/> selected.
        /// </returns>
        ConstructorInjectionRequest RequestDependencyInjection(Type type);

        /// <summary>
        /// Gets <see cref="FieldInjectionRequest"/> for <see cref="Type"/>.
        /// </summary>
        /// <param name="type">
        /// <see cref="Type"/> for request.
        /// </param>
        /// <returns>
        /// <see cref="FieldInjectionRequest"/> selected.
        /// </returns>
        FieldInjectionRequest RequestFieldInjection(Type type);

        /// <summary>
        /// Gets <see cref="PropertyInjectionRequest"/> for <see cref="Type"/>.
        /// </summary>
        /// <param name="type">
        /// <see cref="Type"/> for request.
        /// </param>
        /// <returns>
        /// <see cref="PropertyInjectionRequest"/> selected.
        /// </returns>
        PropertyInjectionRequest RequestPropertyInjection(Type type);

        /// <summary>
        /// Gets <see cref="MethodInjectionRequest"/> for <see cref="Type"/>.
        /// </summary>
        /// <param name="type">
        /// <see cref="Type"/> for request.
        /// </param>
        /// <returns>
        /// <see cref="MethodInjectionRequest"/> selected.
        /// </returns>
        MethodInjectionRequest RequestMethodInjection(Type type);

        /// <summary>
        /// Gets <see cref="ConstructorInjectionRequest"/> for <see cref="Type"/>.
        /// </summary>
        /// <param name="elementType">
        /// <see cref="Type"/> of element for request.
        /// </param>
        /// <returns>
        /// <see cref="ConstructorInjectionRequest"/> selected.
        /// </returns>
        ConstructorInjectionRequest RequestServiceBundleInjection(Type elementType);
    }
}
