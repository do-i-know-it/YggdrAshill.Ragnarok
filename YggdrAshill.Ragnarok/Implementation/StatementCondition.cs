using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class StatementCondition : IStatementCondition
    {
        private readonly Func<IStatement, bool> condition;

        public StatementCondition(Func<IStatement, bool> condition)
        {
            this.condition = condition;
        }

        public bool IsSatisfied(IStatement statement)
        {
            return condition.Invoke(statement);
        }
    }
}
