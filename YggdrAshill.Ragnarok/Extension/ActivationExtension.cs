using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class ActivationExtension
    {
        public static IInstantiation ToInstantiate(this IActivation activation)
        {
            return new ActivateToInstantiateWithoutParameterList(activation);
        }

        public static IInstantiation ToInstantiate(this IActivation activation, IReadOnlyList<IParameter> parameterList)
        {
            return new ActivateToInstantiateWithParameterList(activation, parameterList);
        }
    }
}
