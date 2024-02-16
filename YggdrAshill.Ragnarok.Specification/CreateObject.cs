namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class CreateObject : ICreation<object>
    {
        public static CreateObject Instance { get; } = new CreateObject();

        private CreateObject()
        {

        }

        public object Create()
        {
            return new object();
        }
    }
}
