using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to resolve dependencies automatically.
    /// </summary>
    public interface ICompilation
    {
        /// <summary>
        /// Obtains how to instantiate with dependencies resolved.
        /// </summary>
        /// <param name="type">
        /// <see cref="Type"/> to instantiate.
        /// </param>
        /// <param name="parameterList">
        /// <see cref="IParameter"/>s to instantiate.
        /// </param>
        /// <returns>
        /// <see cref="IInstantiation"/> to resolve dependencies automatically.
        /// </returns>
        IInstantiation GetInstantiation(Type type, IReadOnlyList<IParameter> parameterList);

        /// <summary>
        /// Obtains how to inject dependencies into fields.
        /// </summary>
        /// <param name="type">
        /// <see cref="Type"/> to inject.
        /// </param>
        /// <param name="parameterList">
        /// <see cref="IParameter"/>s to inject.
        /// </param>
        /// <returns>
        /// <see cref="IInjection"/> to resolve dependencies automatically.
        /// </returns>
        IInjection GetFieldInjection(Type type, IReadOnlyList<IParameter> parameterList);

        /// <summary>
        /// Obtains how to inject dependencies into properties.
        /// </summary>
        /// <param name="type">
        /// <see cref="Type"/> to inject.
        /// </param>
        /// <param name="parameterList">
        /// <see cref="IParameter"/>s to inject.
        /// </param>
        /// <returns>
        /// <see cref="IInjection"/> to resolve dependencies automatically.
        /// </returns>
        IInjection GetPropertyInjection(Type type, IReadOnlyList<IParameter> parameterList);

        /// <summary>
        /// Obtains how to inject dependencies into methods.
        /// </summary>
        /// <param name="type">
        /// <see cref="Type"/> to inject.
        /// </param>
        /// <param name="parameterList">
        /// <see cref="IParameter"/>s to inject.
        /// </param>
        /// <returns>
        /// <see cref="IInjection"/> to resolve dependencies automatically.
        /// </returns>
        IInjection GetMethodInjection(Type type, IReadOnlyList<IParameter> parameterList);
    }
}
