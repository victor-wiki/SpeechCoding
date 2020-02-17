using WindowsInput.Native;

namespace SpeechCodingHandler
{
    public class JavaInterpreter : LanguageInterpreter
    {
        protected override string relativeFileExtension => ".jar";
        public override string Language => "Java";      
    }
}
