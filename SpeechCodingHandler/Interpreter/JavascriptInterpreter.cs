using WindowsInput.Native;

namespace SpeechCodingHandler
{
    public class JavascriptInterpreter : LanguageInterpreter
    {
        protected override string relativeFileExtension => ".js";
        public override string Language => "JavaScript";        
    }
}
