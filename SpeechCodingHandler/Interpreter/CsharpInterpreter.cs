using WindowsInput.Native;

namespace SpeechCodingHandler
{
    public class CsharpInterpreter:LanguageInterpreter
    {
        protected override string relativeFileExtension => ".cs;.csproj;.dll";

        public override string Language => "C#";       
    }
}
