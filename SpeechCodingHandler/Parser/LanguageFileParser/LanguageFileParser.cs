namespace SpeechCodingHandler
{
    public abstract class LanguageFileParser
    {
        public abstract string FileExtension { get; }
        public string FilePath { get; set; }

        public LanguageFileParser() { }

        public LanguageFileParser(string filePath)
        {
            this.FilePath = filePath;
        }

        public abstract LanguageFileInfo Parse();
    }
}
