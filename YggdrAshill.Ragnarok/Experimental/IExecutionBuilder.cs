using YggdrAshill.Ragnarok.Periodization;

namespace YggdrAshill.Ragnarok.Experimental
{
    internal interface IExecutionBuilder
    {
        IExecutionBuilder Configure(IExecution execution);

        IExecution Build();
    }
}
