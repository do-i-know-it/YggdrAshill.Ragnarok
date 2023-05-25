using System;

namespace YggdrAshill.Ragnarok.Fabrication
{
    public interface IAssignImplementedInterface :
        IAssignImplementedType
    {
        IAssignImplementedInterface As(Type implementedInterface);
    }
}
