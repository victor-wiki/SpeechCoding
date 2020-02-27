using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechCodingHandler
{
    public class LanguageFileInfo
    {
        public FileInfo FileInfo { get; set; }

        public List<ObjectInfo> ObjectInfos { get; set; } = new List<ObjectInfo>();
    }
}
