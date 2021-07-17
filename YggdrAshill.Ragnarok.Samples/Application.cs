using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok.Samples
{
    internal sealed class Application :
        IProcession
    {
        internal bool runninng;

        public void Originate()
        {
            Console.WriteLine("Originated.");
            runninng = true;
        }

        public void Terminate()
        {
            Console.WriteLine("Terminated.");
            Environment.Exit(0);
        }

        public void Execute()
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
                runninng = false;
            }

            Console.WriteLine($"Executed: {input}");
        }
    }
}
