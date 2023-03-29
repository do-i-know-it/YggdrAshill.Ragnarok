namespace YggdrAshill.Ragnarok.Fabrication
{
    public interface IAfterAnyTypeAssigned
    {
        IAfterAnyTypeAssigned As<T>() where T : notnull;
        void AsSelf();
    }
}
