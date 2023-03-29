using System;

namespace YggdrAshill.Ragnarok.Samples
{
    internal sealed class ConsoleReceiver :
        IReceiver
    {
        public static ConsoleReceiver Instance { get; } = new ConsoleReceiver();

        private ConsoleReceiver()
        {

        }

        public void Receive(string message)
        {
            Console.WriteLine(message);
        }
    }
}
