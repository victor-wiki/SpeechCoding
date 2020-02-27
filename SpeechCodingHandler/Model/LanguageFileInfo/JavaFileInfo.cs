using System.Collections.Generic;
using System.IO;

namespace SpeechCodingHandler
{
    public class JavaFileInfo : LanguageFileInfo
    {       
        public string PackageName { get; set; }
        public List<string> Imports = new List<string>();       
    }
}
