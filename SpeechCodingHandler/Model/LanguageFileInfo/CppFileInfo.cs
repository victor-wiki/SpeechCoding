using System.Collections.Generic;

namespace SpeechCodingHandler
{
    public class CppFileInfo : LanguageFileInfo
    {
        public List<string> Usings { get; set; } = new List<string>();
    }
}
