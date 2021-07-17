using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok.Samples
{
    /// <summary>
    /// <see cref="IAbortion"/> for <see cref="ConsoleApplicationForSample"/>.
    /// </summary>
    internal sealed class AbortOnConsoleApplicationForSample :
        IAbortion
    {
        public void Abort(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            Console.WriteLine($"Errored: {exception}");

            throw exception;
        }
    }
}
