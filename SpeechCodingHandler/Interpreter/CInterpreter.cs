using WindowsInput.Native;

namespace SpeechCodingHandler
{
    public class CInterpreter : LanguageInterpreter
    {
        protected override string relativeFileExtension => ".c";
        public override string Language => "C";       
    }
}
