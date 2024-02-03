using System;
using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok
{
    internal sealed class Registration : IRegistration
    {
        private readonly List<IStatement> statementList = new();
        private readonly List<IOperation> operationList = new();
        private readonly List<IDisposable> disposableList = new();

        public int Count(IStatementSelection selection)
        {
            return statementList.Where(selection.IsSatisfied).Count();
        }

        public void Register(IStatement statement)
        {
            if (statementList.Contains(statement))
            {
                return;
            }

            statementList.Add(statement);
        }

        public void Register(IOperation operation)
        {
            if (operationList.Contains(operation))
            {
                return;
            }

            operationList.Add(operation);
        }

        public void Register(IDisposable disposable)
        {
            if (disposableList.Contains(disposable))
            {
                return;
            }

            disposableList.Add(disposable);
        }

        public void Build(IScopedResolverBuilder builder)
        {
            var resolver = builder.Build(statementList);

            foreach (var disposable in disposableList)
            {
                resolver.Bind(disposable);
            }

            foreach (var operation in operationList)
            {
                operation.Operate(resolver);
            }
        }
    }
}
