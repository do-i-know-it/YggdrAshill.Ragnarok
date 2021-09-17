using YggdrAshill.Ragnarok.Periodization;
using System;

namespace YggdrAshill.Ragnarok.Samples
{
    internal sealed class Program
    {
        private static void Main(string[] arguments)
        {
            var application
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

            using (application.Scope())
            {
                application.Execute();
            }
        }
    }
}
