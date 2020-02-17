using SpeechCodingHandler;
using System.Collections.Generic;

namespace SpeechCoding
{
    public class ProjectInfo
    {
        public GrammarType GrammarType { get; set; }
        public List<string> FilePaths { get; set; } = new List<string>();
    }
}
