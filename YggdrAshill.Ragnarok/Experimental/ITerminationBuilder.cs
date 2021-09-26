using YggdrAshill.Ragnarok.Periodization;

namespace YggdrAshill.Ragnarok.Experimental
{
    internal interface ITerminationBuilder
    {
        ITerminationBuilder Configure(ITermination termination);

        ITermination Build();
    }
}
