using System;

namespace YggdrAshill.Ragnarok.Materialization
{
    /// <summary>
    /// Defines how to resolve dependencies automatically.
    /// </summary>
    public interface ICodeBuilder
    {
        /// <summary>
        /// Gets <see cref="IActivation"/> of <see cref="Type"/> to instantiate with dependencies resolved.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IActivation GetActivation(Type type);

        /// <summary>
        /// Gets <see cref="IInfusion"/> of <see cref="Type"/> to inject dependencies into fields.
        /// </summary>
        /// <param name="type">
        /// <see cref="Type"/> to inject dependencies into.
        /// </param>
        /// <returns>
        /// <see cref="IInfusion"/> to inject into instance.
        /// </returns>
        IInfusion GetFieldInfusion(Type type);

        /// <summary>
        /// Gets <see cref="IInfusion"/> of <see cref="Type"/> to inject dependencies into properties.
        /// </summary>
        /// <param name="type">
        /// <see cref="Type"/> to inject dependencies into.
        /// </param>
        /// <returns>
        /// <see cref="IInfusion"/> to inject into instance.
        /// </returns>
        IInfusion GetPropertyInfusion(Type type);

        /// <summary>
        /// Gets <see cref="IInfusion"/> of <see cref="Type"/> to inject dependencies into method.
        /// </summary>
        /// <param name="type">
        /// <see cref="Type"/> to inject dependencies into.
        /// </param>
        /// <returns>
        /// <see cref="IInfusion"/> to inject into instance.
        /// </returns>
        IInfusion GetMethodInfusion(Type type);
    }
}
