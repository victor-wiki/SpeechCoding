using WindowsInput.Native;

namespace SpeechCodingHandler
{
    public class MySqlInterpreter:LanguageInterpreter
    {
        protected override string relativeFileExtension => ".sql";
        public override string Language => "MySQL";         
    }
}
