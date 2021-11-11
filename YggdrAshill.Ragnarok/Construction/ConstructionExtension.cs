using YggdrAshill.Ragnarok.Construction;
using System;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Defines extensions for Construction.
    /// </summary>
    public static class ConstructionExtension
    {
        /// <summary>
        /// Adds <see cref="Action"/> to initialize.
        /// </summary>
        /// <param name="origination">
        /// <see cref="Action"/> to initialize.
        /// </param>
        /// <returns>
        /// <see cref="IService"/> added <paramref name="origination"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="origination"/> is null.
        /// </exception>
        public static IService OnOriginated(this IService service, Action origination)
        {
            if (service is null)
            {
                throw new ArgumentNullException(nameof(origination));
            }
            if (origination is null)
            {
                throw new ArgumentNullException(nameof(origination));
            }

            return service.Configure(Origination.Of(origination));
        }

        /// <summary>
        /// Adds <see cref="Action"/> to finalize.
        /// </summary>
        /// <param name="termination">
        /// <see cref="Action"/> to finalize.
        /// </param>
        /// <returns>
        /// <see cref="IService"/> added <paramref name="termination"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="termination"/> is null.
        /// </exception>
        public static IService OnTerminated(this IService service, Action termination)
        {
            if (service is null)
            {
                throw new ArgumentNullException(nameof(termination));
            }
            if (termination is null)
            {
                throw new ArgumentNullException(nameof(termination));
            }

            return service.Configure(Termination.Of(termination));
        }

        public static IService OnExecuted(this IService service, Action execution)
        {
            if (service is null)
            {
                throw new ArgumentNullException(nameof(execution));
            }
            if (execution is null)
            {
                throw new ArgumentNullException(nameof(execution));
            }

            return service.Configure(Execution.Of(execution));
        }

        /// <summary>
        /// Adds <see cref="Action"/> to initialize and finalize.
        /// </summary>
        /// <param name="origination">
        /// <see cref="Action"/> to initialize.
        /// </param>
        /// <param name="termination">
        /// <see cref="Action"/> to finalize.
        /// </param>
        /// <returns>
        /// <see cref="IService"/> added <paramref name="origination"/> and <paramref name="termination"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="origination"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="termination"/> is null.
        /// </exception>
        public static IService InSpan(this IService service, Action origination, Action termination)
        {
            if (service is null)
            {
                throw new ArgumentNullException(nameof(origination));
            }
            if (origination is null)
            {
                throw new ArgumentNullException(nameof(origination));
            }
            if (termination is null)
            {
                throw new ArgumentNullException(nameof(termination));
            }

            return service.Configure(Origination.Of(origination).To(termination));
        }
    }
}
