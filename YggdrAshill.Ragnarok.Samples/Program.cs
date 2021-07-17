using YggdrAshill.Ragnarok.Progression;

namespace YggdrAshill.Ragnarok.Samples
{
    internal sealed class Program
    {
        private static void Main(string[] arguments)
        {
            var application = new ConsoleApplicationForSample();

            var origination = application.Origination().Bind(new AbortOnConsoleApplicationForSample());
            var execution = application.Execution().Bind(new AbortOnConsoleApplicationForSample());
            var termination = application.Termination().Bind(new AbortOnConsoleApplicationForSample());

            using (termination.ToDisposable())
            {
                origination.Originate();

                execution.Execute();
            }
        }
    }
}
