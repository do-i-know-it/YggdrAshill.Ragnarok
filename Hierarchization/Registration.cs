using System;
using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok
{
    internal sealed class Registration : IRegistration
    {
        private readonly IScopedResolverContext context;

        public Registration(IScopedResolverContext context)
        {
            this.context = context;
        }

        private readonly List<IStatement> statementList = new();
        private readonly List<IExecution> executionList = new();
        private readonly List<IDisposable> disposableList = new();

        public int Count(IStatementCondition condition)
        {
            return statementList.Where(condition.IsSatisfied).Count();
        }

        public void Register(IStatement statement)
        {
            if (statementList.Contains(statement))
            {
                return;
            }

            statementList.Add(statement);
        }

        public void Register(IExecution execution)
        {
            if (executionList.Contains(execution))
            {
                return;
            }

            executionList.Add(execution);
        }

        public void Register(IDisposable disposable)
        {
            if (disposableList.Contains(disposable))
            {
                return;
            }

            disposableList.Add(disposable);
        }

        public IScopedResolver Build()
        {
            var resolver = context.Build(statementList);

            foreach (var disposable in disposableList)
            {
                resolver.Bind(disposable);
            }

            foreach (var operation in executionList)
            {
                operation.Execute(resolver);
            }

            return resolver;
        }
    }
}
