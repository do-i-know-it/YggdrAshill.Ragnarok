using YggdrAshill.Ragnarok.Periodization;
using YggdrAshill.Ragnarok.Experimental;
using System;

namespace YggdrAshill.Ragnarok.Samples
{
    /// <summary>
    /// Defines entry point for the sample application.
    /// </summary>
    internal sealed class Program
    {
        /// <summary>
        /// Entry point for the application.
        /// </summary>
        /// <param name="arguments">
        /// <see cref="string[]"/> received from command line.
        /// </param>
        private static void Main(string[] arguments)
        {
            PeriodizedService
                .Default
                .OnOriginated(() =>
                {
                    // define how to initialize this application.
                    Console.WriteLine("Originated.");
                })
                .OnExecuted(() =>
                {
                    Console.WriteLine();
                    Console.WriteLine($"Please enter some text.");
                    Console.WriteLine($"If you want to quit this application, enter \"Exit\".");

                    // define a loop for this application.
                    while (true)
                    {
                        var input = Console.ReadLine();

                        if (input.ToLower() == "exit")
                        {
                            return;
                        }

                        Console.WriteLine($"Executed: {input}");
                    }
                })
                .OnTerminated(() =>
                {
                    // define how to finalize this application.
                    Console.WriteLine("Terminated.");
                })
                .Run();
        }
    }
}
