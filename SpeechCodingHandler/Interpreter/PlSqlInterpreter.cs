using WindowsInput.Native;

namespace SpeechCodingHandler
{
    public class PlSqlInterpreter : LanguageInterpreter
    {
        protected override string relativeFileExtension => ".sql";
        public override string Language => "PLSQL";           
    }
}
