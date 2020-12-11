using System;

namespace YggdrAshill.Ragnarok.Administration
{
    public interface IAbortion
    {
        void Abort(Exception exception);
    }
}
