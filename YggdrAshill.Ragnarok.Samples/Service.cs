using System;

namespace YggdrAshill.Ragnarok.Samples
{
    internal sealed class Service : IService
    {
        private readonly ISender sender;
        private readonly IFormatter formatter;
        private readonly IReceiver receiver;

        [Inject]
        public Service(ISender sender, IFormatter formatter, IReceiver receiver)
        {
            this.sender = sender;
            this.formatter = formatter;
            this.receiver = receiver;
        }

        [InjectProperty] private string? Announcement { get; set; }

        public void Run()
        {
            if (Announcement != null)
            {
                Console.WriteLine(Announcement);
            }

            while (true)
            {
                var message = sender.Send();

                if (message.ToLower() == "quit")
                {
                    Console.WriteLine("quit sample application.");

                    break;
                }

                var formatted = formatter.Format(message);

                receiver.Receive(formatted);
            }
        }
    }
}
