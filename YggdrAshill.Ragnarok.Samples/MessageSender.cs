namespace YggdrAshill.Ragnarok.Samples
{
    internal sealed class MessageSender :
        ISender
    {
        private readonly string message;

        [Inject]
        public MessageSender(string message)
        {
            this.message = message;
        }

        public string Send()
        {
            return message;
        }
    }
}
