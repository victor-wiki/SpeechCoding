using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SpeechCodingHandler
{
    public class CsharpProjectFileInfo
    {
        public FileInfo FileInfo { get; set; }
        public string Configuration { get; set; }
        public string TargetFrameworkVersion { get; set; }
        public string PlatformTarget { get; set; }
        public string AssemblyName { get; set; }
        public string OutputPath { get; set; }
        public string OutputType { get; set; }

        public List<AssemblyReferenceInfo> References { get; set; } = new List<AssemblyReferenceInfo>();
        public List<string> CsFilePaths = new List<string>();
    }    
}
