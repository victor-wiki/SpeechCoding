using WindowsInput.Native;

namespace SpeechCodingHandler
{
    public class ObjectiveCInterpreter : LanguageInterpreter
    {
        protected override string relativeFileExtension => ".m";
        public override string Language => "Objective-C";       
    }
}
