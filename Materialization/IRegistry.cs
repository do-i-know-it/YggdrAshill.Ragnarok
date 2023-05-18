using YggdrAshill.Ragnarok.Hierarchization;
using System;

namespace YggdrAshill.Ragnarok.Materialization
{
    public interface IRegistry :
        IDisposable
    {
        bool TryGet(Type type, out IRegistration? registration);
    }
}
