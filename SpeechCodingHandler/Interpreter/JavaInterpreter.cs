using WindowsInput.Native;

namespace SpeechCodingHandler
{
    public class JavaInterpreter : LanguageInterpreter
    {
        protected override string relativeFileExtension => ".jar;.java";
        public override string Language => "Java";      
    }
}
