using System;
using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok
{
    internal sealed class Registration : IRegistration
    {
        private readonly IScopedResolverBuilder builder;

        public Registration(IScopedResolverBuilder builder)
        {
            this.builder = builder;
        }

        private readonly List<IStatement> statementList = new();
        private readonly List<IInstruction> instructionList = new();
        private readonly List<IDisposable> disposableList = new();

        public int Count(ICondition condition)
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

        public void Register(IInstruction instruction)
        {
            if (instructionList.Contains(instruction))
            {
                return;
            }

            instructionList.Add(instruction);
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
            var resolver = builder.Build(statementList);

            foreach (var disposable in disposableList)
            {
                resolver.Bind(disposable);
            }

            foreach (var operation in instructionList)
            {
                operation.Execute(resolver);
            }

            return resolver;
        }
    }
}
