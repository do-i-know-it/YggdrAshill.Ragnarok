﻿using YggdrAshill.Ragnarok.Periodization;

namespace YggdrAshill.Ragnarok.Unification
{
    public interface ITerminationCollection
    {
        void Collect(ITermination termination);
    }
}
