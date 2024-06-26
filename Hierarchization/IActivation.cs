﻿namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to create instance with dependencies.
    /// </summary>
    public interface IActivation
    {
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
