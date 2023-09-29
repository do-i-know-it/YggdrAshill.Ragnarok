using System;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public interface IRegistrationV2
    {
        void Register(IDescriptionV2 description);
        void Register(IOperation operation);
        void Register(IDisposable disposable);
    }
}
