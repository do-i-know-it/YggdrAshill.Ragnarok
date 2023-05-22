using System;

namespace YggdrAshill.Ragnarok.Samples
{
    internal sealed class Formatter :
        IFormatter
    {
        public static Formatter AllCharactersToUpper { get; } = new Formatter();

        private Formatter()
        {

        }

        public string Format(string original)
        {
            var formatted = original.ToUpper();

            Console.WriteLine($"Before: {original}, After: {formatted}");

            return formatted;
        }
    }
}
