using YggdrAshill.Ragnarok.Periodization;

namespace YggdrAshill.Ragnarok.Experimental
{
    internal interface IOriginationBuilder
    {
        IOriginationBuilder Configure(IOrigination origination);

        IOrigination Build();
    }
}
