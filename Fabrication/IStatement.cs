using YggdrAshill.Ragnarok.Construction;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Fabrication
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
