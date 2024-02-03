namespace YggdrAshill.Ragnarok
{
    internal sealed class InjectIntoNothing : IInjection
    {
        public static InjectIntoNothing Instance { get; } = new();

        private InjectIntoNothing()
        {

        }

        public void Inject(IObjectResolver resolver, object instance)
        {

        }
    }
}
