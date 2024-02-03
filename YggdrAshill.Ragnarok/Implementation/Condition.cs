using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class Condition : ICondition
    {
        private readonly Func<IStatement, bool> condition;

        public Condition(Func<IStatement, bool> condition)
        {
            this.condition = condition;
        }

        public bool IsSatisfied(IStatement statement)
        {
            return condition.Invoke(statement);
        }
    }
}
