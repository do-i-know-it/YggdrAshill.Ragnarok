﻿using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to activate or infuse instance.
    /// </summary>
    public interface IOperation
    {
        /// <summary>
        /// Creates <see cref="IActivation"/> for <see cref="ConstructorInjectionRequest"/>.
        /// </summary>
        /// <param name="request">
        /// <see cref="ConstructorInjectionRequest"/> for request.
        /// </param>
        /// <returns>
        /// <see cref="IActivation"/> created.
        /// </returns>
        IActivation CreateActivation(ConstructorInjectionRequest request);

        /// <summary>
        /// Creates <see cref="IInfusion"/> for <see cref="FieldInjectionRequest"/>.
        /// </summary>
        /// <param name="request">
        /// <see cref="FieldInjectionRequest"/> for request.
        /// </param>
        /// <returns>
        /// <see cref="IInfusion"/> created.
        /// </returns>
        IInfusion CreateFieldInfusion(FieldInjectionRequest request);

        /// <summary>
        /// Creates <see cref="IInfusion"/> for <see cref="PropertyInjectionRequest"/>.
        /// </summary>
        /// <param name="request">
        /// <see cref="PropertyInjectionRequest"/> for request.
        /// </param>
        /// <returns>
        /// <see cref="IInfusion"/> created.
        /// </returns>
        IInfusion CreatePropertyInfusion(PropertyInjectionRequest request);

        /// <summary>
        /// Creates <see cref="IInfusion"/> for <see cref="MethodInjectionRequest"/>.
        /// </summary>
        /// <param name="request">
        /// <see cref="MethodInjectionRequest"/> for request.
        /// </param>
        /// <returns>
        /// <see cref="IInfusion"/> created.
        /// </returns>
        IInfusion CreateMethodInfusion(MethodInjectionRequest request);

        /// <summary>
        /// Creates <see cref="IActivation"/> for collection of <see cref="Type"/>.
        /// </summary>
        /// <param name="request">
        /// <see cref="CollectionInjectionRequest"/> for request.
        /// </param>
        /// <returns>
        /// <see cref="IActivation"/> created.
        /// </returns>
        IActivation CreateCollectionActivation(CollectionInjectionRequest request);
    }
}
