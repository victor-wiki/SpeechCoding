namespace SpeechCodingHandler
{
    public class HtmlInterpreter : LanguageInterpreter
    {
        protected override string relativeFileExtension => ".html;.htm";
        public override string Language => "HTML";       
    }
}
