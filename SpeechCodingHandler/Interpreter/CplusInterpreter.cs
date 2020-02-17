using WindowsInput.Native;

namespace SpeechCodingHandler
{
    public class CplusInterpreter : LanguageInterpreter
    {
        protected override string relativeFileExtension => ".cpp";
        public override string Language => "C++";       
    }
}
