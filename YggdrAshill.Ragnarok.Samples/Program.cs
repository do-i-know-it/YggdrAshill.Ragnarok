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
            Service
                .Default
                .OnOriginated(() =>
                {
                    // define how to initialize this application.
                    Console.WriteLine("Originated.");
                })
                .OnExecuted(() =>
                {
                    // define how to execute this application.
                    Console.WriteLine($"Executed.");
                })
                .OnTerminated(() =>
                {
                    // define how to finalize this application.
                    Console.WriteLine("Terminated.");
                })
                .InSpan(() =>
                {
                    // define how to initialize this application.
                    Console.WriteLine("Span opened.");
                }, () =>
                {
                    // define how to finalize this application.
                    Console.WriteLine("Span closed.");
                })
                .Build()
                .Run();
        }
    }
}
