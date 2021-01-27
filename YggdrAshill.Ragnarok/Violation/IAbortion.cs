using System;

namespace YggdrAshill.Ragnarok.Violation
{
    public interface IAbortion
    {
        void Abort(Exception exception);
    }
}
