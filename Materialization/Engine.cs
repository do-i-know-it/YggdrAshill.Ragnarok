using YggdrAshill.Ragnarok.Hierarchization;
using YggdrAshill.Ragnarok.Motorization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Materialization
{
    internal sealed class Engine :
        IEngine
    {
        private readonly Dictionary<Type, IRegistration> dictionary;

        public Engine(IDictionary<Type, IRegistration> table)
        {
            dictionary = new Dictionary<Type, IRegistration>(table);
        }

        private bool isDisposed;

        private readonly ConcurrentDictionary<IRegistration, object> cache = new ConcurrentDictionary<IRegistration, object>();

        private readonly HashSet<IDisposable> disposableList = new HashSet<IDisposable>();

        public bool Have(IRegistration registration)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IEngine));
            }

            return dictionary.ContainsValue(registration);
        }

        public bool Find(Type type, out IRegistration registration)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IEngine));
            }

            return dictionary.TryGetValue(type, out registration);
        }

        public object GetInstance(IRegistration registration, Func<IRegistration, object> factory)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IEngine));
            }

            return cache.GetOrAdd(registration, factory);
        }

        public void Bind(IDisposable disposable)
        {
            lock (disposableList)
            {
                disposableList.Add(disposable);
            }
        }

        public void Dispose()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IEngine));
            }

            lock (dictionary)
            {
                dictionary.Clear();
            }

            cache.Clear();

            lock (disposableList)
            {
                foreach (var disposable in disposableList)
                {
                    disposable.Dispose();
                }

                disposableList.Clear();
            }

            isDisposed = true;
        }
    }
}
