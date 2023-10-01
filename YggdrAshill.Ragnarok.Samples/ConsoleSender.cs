using System;

namespace YggdrAshill.Ragnarok.Samples
{
    internal sealed class ConsoleSender : ISender
    {
        private readonly string announcement;

        [Inject]
        private ConsoleSender(string announcement)
        {
            this.announcement = announcement;
        }

        public string Send()
        {
            Console.Write($"{announcement}:");

            return Console.ReadLine() ?? string.Empty;
        }
    }
}
