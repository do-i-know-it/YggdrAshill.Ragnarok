using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class RealizeWithoutParameterList : IRealization
    {
        private readonly IReadOnlyList<Argument> argumentList;

        public RealizeWithoutParameterList(IReadOnlyList<Argument> argumentList)
        {
            this.argumentList = argumentList;
        }

        public object[] Realize(IObjectResolver resolver)
        {
            var instanceList = new object[argumentList.Count];

            for (var index = 0; index < argumentList.Count; index++)
            {
                var argument = argumentList[index];

                instanceList[index] = resolver.Resolve(argument.Type);
            }

            return instanceList;
        }
    }
}
