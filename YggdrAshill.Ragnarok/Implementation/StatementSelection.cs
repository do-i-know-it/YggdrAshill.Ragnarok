using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class StatementSelection : IStatementSelection
    {
        private readonly Func<IStatement, bool> condition;

        public StatementSelection(Func<IStatement, bool> condition)
        {
            this.condition = condition;
        }

        public bool IsSatisfied(IStatement statement)
        {
            return condition.Invoke(statement);
        }
    }
}
