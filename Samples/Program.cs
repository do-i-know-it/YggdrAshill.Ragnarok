using System;

namespace YggdrAshill.Ragnarok.Samples
{
    /// <summary>
    /// Sample entry point to show how to use this framework.
    /// </summary>
    internal sealed class Program
    {
        internal static void Main(string[] arguments)
        {
            // Defines how to initialize sample application.
            var origination = new Origination(() =>
            {
                Console.WriteLine("Originated.");

                return new Termination(() =>
                {
                    Console.WriteLine("Terminated.");
                    Environment.Exit(0);
                });
            });

            // Initializes this application, and gets token to finalize it.
            var termination = origination.Originate();


            // Defines how to set up game loop execution.
            var activation = new Activation(() =>
            {
                Console.WriteLine("Activated.");

                return new Execution(() =>
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
                        termination.Terminate();
                    }

                    Console.WriteLine($"Executed: {input}");
                });
            });

            // Activates game loop, and gets token to execute.
            var execution 
                = activation.Activate()
                .Bind(exception =>
                {
                    Console.WriteLine($"Errored: {exception}");
                    Environment.Exit(-1);
                });

            while (true)
            {
                execution.Execute();
            }
        }
    }
}
