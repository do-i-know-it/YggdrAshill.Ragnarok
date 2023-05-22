namespace YggdrAshill.Ragnarok.Fabrication
{
    public interface IAfterAnyTypeAssigned
    {
        IAfterAnyTypeAssigned And<T>() where T : notnull;

        void AndSelf();
    }
}
