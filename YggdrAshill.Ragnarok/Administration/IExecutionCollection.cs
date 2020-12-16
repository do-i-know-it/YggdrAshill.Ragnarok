using YggdrAshill.Ragnarok.Administration;
using YggdrAshill.Ragnarok.Periodization;

namespace YggdrAshill.Ragnarok
{
    public interface IExecutionCollection
    {
        ITermination Collect(IExecution execution);
    }
}
