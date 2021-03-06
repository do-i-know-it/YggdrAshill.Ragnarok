﻿using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok.Violation
{
    internal sealed class ExecutionWithAbortion :
        IExecution
    {
        private readonly IExecution execution;

        private readonly IAbortion abortion;

        public ExecutionWithAbortion(IExecution execution, IAbortion abortion)
        {
            this.execution = execution;

            this.abortion = abortion;
        }

        public void Execute()
        {
            try
            {
                execution.Execute();
            }
            catch (Exception exception)
            {
                abortion.Abort(exception);
            }
        }
    }
}
