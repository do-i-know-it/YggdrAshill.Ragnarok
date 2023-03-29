namespace YggdrAshill.Ragnarok.Fabrication
{
    public interface IAfterImplementedTypeAssigned
    {
        IAfterImplementedTypeAssigned As<T>() where T : notnull;
        void AsImplementedInterfaces();
    }
}
