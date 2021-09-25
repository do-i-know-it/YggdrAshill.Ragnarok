using YggdrAshill.Ragnarok.Periodization;

namespace YggdrAshill.Ragnarok.Experimental
{
    public interface IService
    {
        IService Configure(IOrigination origination);

        IService Configure(ITermination termination);

        IService Configure(IExecution execution);

        IService Configure(ISpan span);

        ICycle Build();
    }
}
