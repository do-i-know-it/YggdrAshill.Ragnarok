using YggdrAshill.Ragnarok.Administration;

namespace YggdrAshill.Ragnarok
{
    public interface IExecutionCollection
    {
        ITermination Collect(IExecution execution);
    }
}
