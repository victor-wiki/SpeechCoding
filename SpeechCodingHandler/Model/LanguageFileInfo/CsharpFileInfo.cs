using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechCodingHandler
{
    public class CsharpFileInfo : LanguageFileInfo
    {
        public string AssemblyName { get; set; }
        public string Namespace { get; set; }
        public List<string> Usings { get; set; } = new List<string>();       

        public CsharpProjectFileInfo ProjectInfo { get; set; }
    }
}
