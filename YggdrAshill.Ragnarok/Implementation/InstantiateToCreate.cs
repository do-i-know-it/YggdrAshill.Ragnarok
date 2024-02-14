using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class InstantiateToCreate<T> : IInstantiation
        where T : notnull
    {
        private readonly ICreation<T> creation;

        public InstantiateToCreate(ICreation<T> creation)
        {
            this.creation = creation;
        }

        public object Instantiate(IObjectResolver resolver)
        {
            return creation.Create();
        }
    }
}
