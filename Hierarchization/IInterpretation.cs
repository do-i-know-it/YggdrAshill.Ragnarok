using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to activate or infuse instance.
    /// </summary>
    public interface IInterpretation
    {
        /// <summary>
        /// Gets <see cref="IActivation"/> for <see cref="Type"/>.
        /// </summary>
        /// <param name="type">
        /// <see cref="Type"/> to get <see cref="IActivation"/>.
        /// </param>
        /// <returns></returns>
        IActivation GetActivation(Type type);

        /// <summary>
        /// Gets <see cref="IInfusion"/> for fields of <see cref="Type"/>.
        /// </summary>
        /// <param name="type">
        /// <see cref="Type"/> to get <see cref="IInfusion"/>.
        /// </param>
        /// <returns>
        /// <see cref="IInfusion"/> for fields.
        /// </returns>
        IInfusion GetFieldInfusion(Type type);

        /// <summary>
        /// Gets <see cref="IInfusion"/> for properties of <see cref="Type"/>.
        /// </summary>
        /// <param name="type">
        /// <see cref="Type"/> to get <see cref="IInfusion"/>.
        /// </param>
        /// <returns>
        /// <see cref="IInfusion"/> for properties.
        /// </returns>
        IInfusion GetPropertyInfusion(Type type);

        /// <summary>
        /// Gets <see cref="IInfusion"/> for method of <see cref="Type"/>.
        /// </summary>
        /// <param name="type">
        /// <see cref="Type"/> to get <see cref="IInfusion"/>.
        /// </param>
        /// <returns>
        /// <see cref="IInfusion"/> for method.
        /// </returns>
        IInfusion GetMethodInfusion(Type type);
    }
}
