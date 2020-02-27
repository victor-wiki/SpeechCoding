using System.Collections.Generic;

namespace SpeechCodingHandler
{
    public class PhpFileInfo : LanguageFileInfo
    {
        public string Namespace { get; set; }
        public List<string> Usings { get; set; } = new List<string>();
    }
}
