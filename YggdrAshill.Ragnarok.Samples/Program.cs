using YggdrAshill.Ragnarok.Periodization;
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
            var plan
                = Execution.Of(() =>
                {
                    // define a loop for this application.
                    while (true)
                    {
                        Console.WriteLine($"\nPlease enter some text.");
                        Console.WriteLine($"When quitting this application, enter \"Exit\".");

                        var input = Console.ReadLine();

                        if (input.ToLower() == "exit")
                        {
                            return;
                        }

                        Console.WriteLine($"Executed: {input}");
                    }
                })
                .In(() =>
                {
                    // define how to initialize this application.
                    Console.WriteLine("Originated.");
                }, () =>
                {
                    // define how to finalize this application.
                    Console.WriteLine("Terminated.");
                });

            using (plan.Scope())
            {
                plan.Execute();
            }
        }
    }
}
