using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    // TODO: rename interface.
    public interface IStatement
    {
        Type ImplementedType { get; }
        IReadOnlyList<Type> AssignedTypeList { get; }
        IInstantiation Instantiation { get; }
    }
}
