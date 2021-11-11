using YggdrAshill.Ragnarok.Periodization;

namespace YggdrAshill.Ragnarok
{
    internal interface IOriginationBuilder
    {
        IOriginationBuilder Configure(IOrigination origination);

        IOrigination Build();
    }
}
