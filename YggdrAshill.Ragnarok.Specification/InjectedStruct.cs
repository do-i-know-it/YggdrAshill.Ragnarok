namespace YggdrAshill.Ragnarok.Specification
{
    internal readonly struct InjectedStruct
    {
        public int Value { get; }

        [Inject]
        public InjectedStruct(int value)
        {
            Value = value;
        }
    }
}
