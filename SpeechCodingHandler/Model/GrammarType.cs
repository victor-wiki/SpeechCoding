using System;

namespace SpeechCodingHandler
{
    [Flags]
    public enum GrammarType
    {
        Unknown = 0,
        Control = 2,
        Keyword = 4,
        Custom = 8,
        General = 16
    }

    public enum GrammarPriority
    {
        Lowest = 1,
        Lower = 2,
        Low = 3,
        Medium = 4,
        High = 5,
        Higher = 6,
        Highest = 7
    }
}
