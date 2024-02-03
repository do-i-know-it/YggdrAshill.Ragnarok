using System;
using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok
{
    internal sealed class WithDependency : IDependency
    {
        private readonly IReadOnlyList<Argument> argumentList;

        public WithDependency(IReadOnlyList<Argument> argumentList)
        {
            this.argumentList = argumentList;
        }

        private Type[]? dependentTypeList;

        public IReadOnlyList<Type> DependentTypeList
        {
            get
            {
                if (dependentTypeList == null)
                {
                    dependentTypeList = argumentList.Select(argument => argument.Type).Distinct().ToArray();
                }

                return dependentTypeList;
            }
        }

        public IRealization CreateRealization(IReadOnlyList<IParameter> parameterList)
        {
            if (parameterList.Count == 0)
            {
                return new RealizeWithoutParameterList(argumentList);
            }

            return new RealizeWithParameterList(argumentList, parameterList);
        }
    }
}
