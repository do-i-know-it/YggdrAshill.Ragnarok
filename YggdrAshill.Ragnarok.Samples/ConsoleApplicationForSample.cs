using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok.Samples
{
    /// <summary>
    /// Defines <see cref="IProcess"/> for sample application.
    /// </summary>
    internal sealed class ConsoleApplicationForSample :
        IProcess
    {
        private bool running;

        public void Originate()
        {
            Console.WriteLine("Originated.");
            running = true;
        }

        public void Execute()
        {

            // In this application, execute game loop.
            while (running)
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
                    running = false;
                }

                Console.WriteLine($"Executed: {input}");
            }
        }

        public void Terminate()
        {
            Console.WriteLine("Terminated.");
            Environment.Exit(0);
        }
    }
}
