using WindowsInput.Native;

namespace SpeechCodingHandler
{
    public class CppInterpreter : LanguageInterpreter
    {
        protected override string relativeFileExtension => ".cpp";
        public override string Language => "C++";       
    }
}
