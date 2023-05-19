using System;

namespace YggdrAshill.Ragnarok.Fabrication
{
    public interface IInjectIntoConstructor :
        IInjectIntoInstance
    {
        [Obsolete("Use WithArgument<T>(string, T) instead.")]
        IInjectIntoConstructor With<T>(string name, T instance) where T : notnull;
        IInjectIntoConstructor WithArgument<T>(string name, T instance) where T : notnull;
    }
}
