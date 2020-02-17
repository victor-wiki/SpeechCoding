using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechCodingHandler
{
    public class CsharpFileInfo
    {
        public FileInfo FileInfo { get; set; }
        public string AssemblyName { get; set; }
        public string Namespace { get; set; }
        public List<string> Usings { get; set; } = new List<string>();
        public List<ObjectInfo> ObjectInfos { get; set; } = new List<ObjectInfo>();

        public CsharpProjectFileInfo ProjectInfo { get; set; }
    }
}
