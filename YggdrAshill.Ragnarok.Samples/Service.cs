namespace YggdrAshill.Ragnarok.Samples
{
    internal sealed class Service
    {
        private readonly ISender sender;
        private readonly IReceiver receiver;

        [Inject]
        public Service(ISender sender, IReceiver receiver)
        {
            this.sender = sender;
            this.receiver = receiver;
        }

        public void Run()
        {
            var message = sender.Send();

            receiver.Receive(message);
        }
    }
}
