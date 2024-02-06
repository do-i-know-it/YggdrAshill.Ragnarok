using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class RealizeWithParameterList : IRealization
    {
        private readonly IReadOnlyList<Argument> argumentList;
        private readonly IReadOnlyList<IParameter> parameterList;

        public RealizeWithParameterList(IReadOnlyList<Argument> argumentList, IReadOnlyList<IParameter> parameterList)
        {
            this.argumentList = argumentList;
            this.parameterList = parameterList;
        }

        public object[] Realize(IObjectResolver resolver)
        {
            var instanceList = new object[argumentList.Count];

            for (var index = 0; index < argumentList.Count; index++)
            {
                var argument = argumentList[index];

                instanceList[index] = Resolve(resolver, argument);
            }

            return instanceList;
        }

        private object Resolve(IObjectResolver resolver, Argument argument)
        {
            foreach (var parameter in parameterList)
            {
                if (parameter.CanResolve(argument, out var instance))
                {
                    return instance;
                }
            }

            return resolver.Resolve(argument.Type);
        }
    }
}
