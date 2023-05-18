using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Hierarchization;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Materialization
{
    public interface IRegistryBuilder :
        ICodeBuilder
    {
        IRegistry Build(IEnumerable<IDescription> descriptionList, out IEnumerable<IRegistration> registrationList);
    }
}
