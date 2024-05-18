namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to inject dependencies into instance.
    /// </summary>
    public interface IInfusion
    {
        /// <summary>
        /// Injects <see cref="object"/>s into instance.
        /// </summary>
        /// <param name="instance">
        /// <see cref="object"/> to inject dependencies into.
        /// </param>
        /// <param name="parameterList">
        /// <see cref="object"/>s to inject instance.
        /// </param>
        void Infuse(ref object instance, object[] parameterList);
    }
}
