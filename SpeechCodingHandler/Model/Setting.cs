using System.Collections.Generic;

namespace SpeechCodingHandler
{
    public class Setting
    {        
        public bool StartRecordWhenStartup { get; set; }
        public string PreferredLanguage { get; set; }

        public List<ControlWordSetting> ControlWords { get; set; } = new List<ControlWordSetting>();
        public List<GrammarPrioritySetting> GrammarPriorities { get; set; } = new List<GrammarPrioritySetting>();
        public List<LanguageSetting> LanguageSettings { get; set; } = new List<LanguageSetting>();
    }    
}
