using YggdrAshill.Ragnarok.Periodization;
using System;

namespace YggdrAshill.Ragnarok.Construction
{
    /// <summary>
    /// Defines extensions for <see cref="IService"/>.
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// Runs built <see cref="IService"/>.
        /// </summary>
        /// <param name="service">
        /// <see cref="IService"/> to run.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="service"/> is null.
        /// </exception>
        public static void Run(this IService service)
        {
            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            service.Build().Run();
        }
    }
}
