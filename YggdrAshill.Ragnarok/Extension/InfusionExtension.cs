using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class InfusionExtension
    {
        public static IInjection ToInject(this IInfusion infusion)
        {
            return new InfuseToInjectWithoutParameterList(infusion);
        }

        public static IInjection ToInject(this IInfusion infusion, IReadOnlyList<IParameter> parameterList)
        {
            return new InfuseToInjectWithParameterList(infusion, parameterList);
        }
    }
}
