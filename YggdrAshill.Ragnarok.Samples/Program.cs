using System;

namespace YggdrAshill.Ragnarok.Samples
{
    internal sealed class Program
    {
        private static void Main(string[] arguments)
        {
            var application = new Application();

            var procession = application.Bind(exception =>
            {
                if (exception == null)
                {
                    throw new ArgumentNullException(nameof(exception));
                }

                Console.WriteLine($"Errored: {exception}");
                Environment.Exit(-1);
            });

            procession.Originate();

            while (application.runninng)
            {
                procession.Execute();
            }

            procession.Terminate();
        }
    }
}
