using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class CompositeDisposable : IDisposable
    {
        private readonly Stack<IDisposable> disposableStack = new();

        public void Dispose()
        {
            while (true)
            {
                IDisposable disposable;

                lock (disposableStack)
                {
                    if (disposableStack.Count == 0)
                    {
                        break;
                    }

                    disposable = disposableStack.Pop();
                }

                disposable.Dispose();
            }
        }

        public void Add(IDisposable disposable)
        {
            lock (disposableStack)
            {
                disposableStack.Push(disposable);
            }
        }
    }
}
