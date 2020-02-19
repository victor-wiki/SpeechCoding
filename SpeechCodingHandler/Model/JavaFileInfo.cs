using System.Collections.Generic;
using System.IO;

namespace SpeechCodingHandler
{
    public class JavaFileInfo
    {
        public FileInfo FileInfo { get; set; }
        public string PackageName { get; set; }
        public List<string> Imports = new List<string>();
        public List<ObjectInfo> ObjectInfos { get; set; } = new List<ObjectInfo>();
    }
}
