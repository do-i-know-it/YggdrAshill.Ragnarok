using YggdrAshill.Ragnarok.Construction;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Materialization
{
    internal sealed class InfuseToInject :
        IInjection
    {
        private readonly IInfusion infusion;
        private readonly IReadOnlyList<IParameter> parameterList;

        public InfuseToInject(IInfusion infusion, IReadOnlyList<IParameter> parameterList)
        {
            this.infusion = infusion;
            this.parameterList = parameterList;
        }

        public void Inject(IResolver resolver, object instance)
        {
            infusion.Infuse(instance, resolver, parameterList);
        }
    }
}
