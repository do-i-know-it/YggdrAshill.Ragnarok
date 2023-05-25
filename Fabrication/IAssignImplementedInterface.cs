using System;

namespace YggdrAshill.Ragnarok.Fabrication
{
    // TODO: add document comments.
    public interface IAssignImplementedInterface :
        IAssignImplementedType
    {
        IAssignImplementedInterface As(Type implementedInterface);
    }
}
