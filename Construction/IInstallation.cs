namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines how to register dependencies into <see cref="IObjectContainer"/>.
    /// </summary>
    public interface IInstallation
    {
        /// <summary>
        /// Installs dependencies into <paramref name="container"/>.
        /// </summary>
        /// <param name="container">
        /// <see cref="IObjectContainer"/> to install.
        /// </param>
        void Install(IObjectContainer container);
    }
}
