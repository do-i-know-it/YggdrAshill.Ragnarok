using YggdrAshill.Ragnarok.Periodization;

namespace YggdrAshill.Ragnarok
{
    internal interface IExecutionBuilder
    {
        IExecutionBuilder Configure(IExecution execution);

        IExecution Build();
    }
}
