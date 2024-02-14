namespace YggdrAshill.Ragnarok
{
    internal sealed class CreateToReturnInstance<T> : ICreation<T>
        where T : notnull
    {
        private readonly T instance;

        public CreateToReturnInstance(T instance)
        {
            this.instance = instance;
        }

        public T Create()
        {
            return instance;
        }
    }
}
