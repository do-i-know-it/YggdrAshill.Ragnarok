using YggdrAshill.Ragnarok.Periodization;

namespace YggdrAshill.Ragnarok.Construction
{
    /// <summary>
    /// Defines how to build <see cref="ICycle"/> from <see cref="IOrigination"/>, <see cref="ITermination"/>, <see cref="IExecution"/> and <see cref="ISpan"/>.
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// Adds <see cref="IOrigination"/> for build.
        /// </summary>
        /// <param name="origination">
        /// <see cref="IOrigination"/> to add.
        /// </param>
        /// <returns>
        /// <see cref="IService"/> added <paramref name="origination"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if <paramref name="origination"/> is null.
        /// </exception>
        IService Configure(IOrigination origination);

        /// <summary>
        /// Adds <see cref="ITermination"/> for build.
        /// </summary>
        /// <param name="termination">
        /// <see cref="ITermination"/> to add.
        /// </param>
        /// <returns>
        /// <see cref="IService"/> added <paramref name="termination"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if <paramref name="termination"/> is null.
        /// </exception>
        IService Configure(ITermination termination);

        /// <summary>
        /// Adds <see cref="IExecution"/> for build.
        /// </summary>
        /// <param name="execution">
        /// <see cref="IExecution"/> to add.
        /// </param>
        /// <returns>
        /// <see cref="IService"/> added <paramref name="execution"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if <paramref name="execution"/> is null.
        /// </exception>
        IService Configure(IExecution execution);

        /// <summary>
        /// Adds <see cref="ISpan"/> for build.
        /// </summary>
        /// <param name="span">
        /// <see cref="ISpan"/> to add.
        /// </param>
        /// <returns>
        /// <see cref="IService"/> added <paramref name="span"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if <paramref name="span"/> is null.
        /// </exception>
        IService Configure(ISpan span);

        /// <summary>
        /// Creates <see cref="ICycle"/> from configuration.
        /// </summary>
        /// <returns>
        /// <see cref="ICycle"/> created.
        /// </returns>
        ICycle Build();
    }
}
