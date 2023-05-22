using System;

namespace YggdrAshill.Ragnarok.Fabrication
{
    public interface IInjectIntoConstructor :
        IInjectIntoInstance
    {
        IInjectIntoConstructor WithArgument<T>(string name, T instance) where T : notnull;
    }
}
