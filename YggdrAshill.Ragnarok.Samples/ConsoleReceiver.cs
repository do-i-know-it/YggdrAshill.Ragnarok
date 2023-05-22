using System;

namespace YggdrAshill.Ragnarok.Samples
{
    internal sealed class ConsoleReceiver :
        IReceiver
    {
        [InjectField] private readonly string? header;

        public void Receive(string message)
        {
            var content = header != null ? $"{header}: {message}" : message;

            Console.WriteLine($"{content}\n");
        }
    }
}
