using System;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public interface IInheritedTypeAssignment : IOwnTypeAssignment
    {
        IInheritedTypeAssignment As(Type inheritedType);
    }
}
