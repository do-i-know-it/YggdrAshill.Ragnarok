namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    /// <summary>
    /// Implementation of <see cref="IComposition"/> with <see cref="IStatement"/>.
    /// </summary>
    public sealed class Composition :
        IComposition
    {
        private readonly Lifetime lifetime;
        private readonly Ownership ownership;
        private readonly IStatement statement;

        public Composition(Lifetime lifetime, Ownership ownership, IStatement statement)
        {
            this.lifetime = lifetime;
            this.ownership = ownership;
            this.statement = statement;
        }

        private IDescription? description;

        public IDescription Compose()
        {
            if (description == null)
            {
                description = new Description(statement, lifetime, ownership);
            }

            return description;
        }
    }
}
