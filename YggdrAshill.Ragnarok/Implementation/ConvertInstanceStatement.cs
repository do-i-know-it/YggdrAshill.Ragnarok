using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ConvertInstanceStatement<TInput, TOutput> : IStatement
        where TInput : notnull
        where TOutput : notnull
    {
        public IInstantiation Instantiation { get; }
        public Ownership Ownership { get; }
        public InstanceInjectionSource Source { get; }

        public ConvertInstanceStatement(IObjectContainer container, Ownership ownership, Func<TInput, TOutput> onConverted)
        {
            Ownership = ownership;
            Source = new InstanceInjectionSource(typeof(TOutput), container);
            Instantiation = new InstantiateToConvert<TInput, TOutput>(onConverted);
        }

        public Type ImplementedType => Source.ImplementedType;

        public IReadOnlyList<Type> AssignedTypeList => Source.AssignedTypeList;

        public Lifetime Lifetime => Lifetime.Temporal;
    }
}
