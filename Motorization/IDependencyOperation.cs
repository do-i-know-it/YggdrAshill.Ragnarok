using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to activate or infuse instance.
    /// </summary>
    public interface IDependencyOperation
    {
        /// <summary>
        /// Creates <see cref="IActivation"/> for <paramref name="request"/>.
        /// </summary>
        /// <param name="request">
        /// <see cref="ConstructorInjectionRequest"/> to create.
        /// </param>
        /// <returns>
        /// <see cref="IActivation"/> created.
        /// </returns>
        IActivation CreateActivation(ConstructorInjectionRequest request);

        /// <summary>
        /// Creates <see cref="IInfusion"/> for <paramref name="request"/>.
        /// </summary>
        /// <param name="request">
        /// <see cref="FieldInjectionRequest"/> to create.
        /// </param>
        /// <returns>
        /// <see cref="IInfusion"/> created.
        /// </returns>
        IInfusion CreateFieldInfusion(FieldInjectionRequest request);

        /// <summary>
        /// Creates <see cref="IInfusion"/> for <paramref name="request"/>.
        /// </summary>
        /// <param name="request">
        /// <see cref="PropertyInjectionRequest"/> to create.
        /// </param>
        /// <returns>
        /// <see cref="IInfusion"/> created.
        /// </returns>
        IInfusion CreatePropertyInfusion(PropertyInjectionRequest request);

        /// <summary>
        /// Creates <see cref="IInfusion"/> for <paramref name="request"/>.
        /// </summary>
        /// <param name="request">
        /// <see cref="MethodInjectionRequest"/> to create.
        /// </param>
        /// <returns>
        /// <see cref="IInfusion"/> created.
        /// </returns>
        IInfusion CreateMethodInfusion(MethodInjectionRequest request);

        /// <summary>
        /// Creates <see cref="IActivation"/> for <paramref name="type"/>.
        /// </summary>
        /// <param name="type">
        /// <see cref="Type"/> to create.
        /// </param>
        /// <returns>
        /// <see cref="IActivation"/> created.
        /// </returns>
        IActivation CreateActivation(Type type);

        /// <summary>
        /// Creates <see cref="IActivation"/> for collection of <paramref name="elementType"/>.
        /// </summary>
        /// <param name="elementType">
        /// <see cref="Type"/> to create.
        /// </param>
        /// <returns>
        /// <see cref="IActivation"/> created.
        /// </returns>
        IActivation CreateCollectionActivation(Type elementType);
    }
}
