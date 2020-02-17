using System;

namespace SpeechCodingHandler
{
    [Flags]
    public enum ObjectType
    {
        Unknown = -1,
        Namespace = 2,
        Interface = 4,
        Class = 8,
        Struct = 16,
        Enum = 32,
        Property = 64,
        Field = 128,
        Method = 256,
        Delegate = 512,
        Event = 1028
    }
}
