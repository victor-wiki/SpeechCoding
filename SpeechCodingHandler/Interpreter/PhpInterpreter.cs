using WindowsInput.Native;

namespace SpeechCodingHandler
{
    public class PhpInterpreter : LanguageInterpreter
    {
        protected override string relativeFileExtension => ".php";
        public override string Language => "PHP";       
    }
}
