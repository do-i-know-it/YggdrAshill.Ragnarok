using System;

namespace YggdrAshill.Ragnarok.Fabrication
{
    public interface IAfterAnyTypeAssigned
    {
        [Obsolete("Use And<T>() instead.")]
        IAfterAnyTypeAssigned As<T>() where T : notnull;

        IAfterAnyTypeAssigned And<T>() where T : notnull;

        [Obsolete("Use AndSelf() instead.")]
        void AsSelf();

        void AndSelf();
    }
}
