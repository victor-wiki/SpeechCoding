using WindowsInput.Native;

namespace SpeechCodingHandler
{
    public class TsqlInterpreter:LanguageInterpreter
    {
        protected override string relativeFileExtension => ".sql";
        public override string Language => "TSQL";        
    }
}
