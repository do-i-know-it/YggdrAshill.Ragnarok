using System;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public interface IRegistration
    {
        int StatementCount { get; }
        int Count(IStatementSelection selection);
        void Register(IStatement statement);
        void Register(IOperation operation);
        void Register(IDisposable disposable);
    }
}