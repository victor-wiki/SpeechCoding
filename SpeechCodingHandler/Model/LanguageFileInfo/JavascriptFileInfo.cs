using System.Collections.Generic;

namespace SpeechCodingHandler
{
    public class JavascriptFileInfo : LanguageFileInfo
    {      
        public List<string> Imports { get; set; } = new List<string>();
        public List<string> Exports { get; set; } = new List<string>();
    }
}
