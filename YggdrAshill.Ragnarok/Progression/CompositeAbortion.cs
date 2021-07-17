using YggdrAshill.Ragnarok.Progression;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// <see cref="IAbortion"/> to aborte each of connected <see cref="IAbortion"/> simultaneously.
    /// </summary>
    public sealed class CompositeAbortion :
        IAbortion,
        IDisposable
    {
        private readonly List<IAbortion> abortionList = new List<IAbortion>();

        internal ITermination Bind(IAbortion abortion)
        {
            if (!abortionList.Contains(abortion))
            {
                abortionList.Add(abortion);
            }

            return Termination.Of(() =>
            {
                if (abortionList.Contains(abortion))
                {
                    abortionList.Remove(abortion);
                }
            });
        }

        /// <inheritdoc/>
        public void Abort(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            foreach (var abortion in abortionList)
            {
                abortion.Abort(exception);
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            abortionList.Clear();
        }
    }
}
