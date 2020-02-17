using WindowsInput.Native;

namespace SpeechCodingHandler
{
    public class PythonInterpreter:LanguageInterpreter
    {
        protected override string relativeFileExtension => ".py";

        public override string Language => "Python";       
    }
}
