using System.Collections.Generic;

namespace SpeechCodingHandler
{
    public class Config
    {
        public List<string> Languages { get; set; } = new List<string>();
        public List<ControlWordSetting> ControlWords { get; set; } = new List<ControlWordSetting>();
    }    
}
