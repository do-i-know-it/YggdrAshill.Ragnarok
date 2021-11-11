using YggdrAshill.Ragnarok.Periodization;

namespace YggdrAshill.Ragnarok
{
    internal interface ITerminationBuilder
    {
        ITerminationBuilder Configure(ITermination termination);

        ITermination Build();
    }
}
