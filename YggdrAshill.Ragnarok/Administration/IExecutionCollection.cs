using YggdrAshill.Ragnarok.Administration;
using YggdrAshill.Ragnarok.Periodization;
using YggdrAshill.Ragnarok.Progression;

namespace YggdrAshill.Ragnarok
{
    public interface IExecutionCollection
    {
        ITermination Collect(IExecution execution);
    }
}
