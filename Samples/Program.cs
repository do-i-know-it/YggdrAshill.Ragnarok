using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok.Samples
{
    /// <summary>
    /// Sample entry point to show how to use this framework.
    /// </summary>
    internal sealed class Program
    {
        private static void Main(string[] arguments)
        {
            // Defines how to initialize sample application.
            var origination = Origination.Of(() =>
            {
                Console.WriteLine("Originated.");
            });

            // Defines how to finalize sample application.
            var termination = Termination.Of(() =>
            {
                Console.WriteLine("Terminated.");
                Environment.Exit(0);
            });

            // Defines how to abort sample application.
            var abortion = Abortion.Of(exception =>
            {
                Console.WriteLine($"Errored: {exception}");
                Environment.Exit(-1);
            });

            var activated = true;

            // Defines how to execute sample application.
            var execution = Execution.Of(() =>
            {
                Console.WriteLine($"");
                Console.WriteLine($"Please enter some text.");
                Console.Write($"\"Exit\" can exit this application:");

                var input = Console.ReadLine();

                if (input.ToLower() == "error")
                {
                    throw new Exception("Error has occurred.");
                }

                if (input.ToLower() == "exit")
                {
                    activated = false;
                }

                Console.WriteLine($"Executed: {input}");
            });

            var loop = execution.Bind(abortion);

            origination.Originate();

            while (activated)
            {
                loop.Execute();
            }

            termination.Terminate();
        }
    }
}
