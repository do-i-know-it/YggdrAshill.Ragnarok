using System;

namespace YggdrAshill.Ragnarok.Fabrication
{
    public interface IAfterImplementedInterfacesAssigned
    {
        [Obsolete("Use AndSelf() instead.")]
        void AsSelf();

        void AndSelf();
    }
}
