using System;

namespace YggdrAshill.Ragnarok.Fabrication
{
    public interface IAssignAnyType
    {
        IAfterAnyTypeAssigned As<T>() where T : notnull;
        [Obsolete]
        IAfterImplementedTypeAssigned AsSelf();
        IAfterImplementedInterfacesAssigned AsImplementedInterfaces();
    }
}
