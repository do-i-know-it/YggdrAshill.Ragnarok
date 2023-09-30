using System;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public interface IRegistration
    {
        void Register(IStatement statement);
        void Register(IOperation operation);
        void Register(IDisposable disposable);
    }
}
