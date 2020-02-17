using Microsoft.Speech.Recognition;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpeechCodingHandler
{
    public delegate void SpeechStateChangeHandler(SpeechState state);
    public delegate void SpeechMessageReceiveHandler(MessageType type, MessageCode messageCode, string message);

    public class SpeechCodingProcessor : IDisposable
    {
        private string cultureName = "en-US";
        private SpeechRecognitionEngine engine;
        private bool stoppedManually = false;

        private Dictionary<GrammarType, string[]> dictGrammarWords = new Dictionary<GrammarType, string[]>();
        private LanguageInterpreter interpreter = null;
        private List<string> needParsePlainTextFileExtensions = new List<string>() { ".cs", ".csproj" };
        public List<string> RelativeFileExtensions = new List<string>() { ".txt" };

        public Dictionary<GrammarType, string[]> GrammarWords => this.dictGrammarWords;

        private string Language
        {
            get
            {
                return string.IsNullOrEmpty(this.Setting.PreferredLanguage) ? LanguageInterpreterHelper.CommonLanguage : this.Setting.PreferredLanguage;
            }
        }

        public bool RecordEnded => this.stoppedManually;

        public Setting Setting { get; private set; } = new Setting();

        public IEnumerable<string> BaseWords => this.dictGrammarWords.Where(item => item.Key != GrammarType.General).Select(item => item.Value).SelectMany(item => item).Distinct();

        public SpeechStateChangeHandler OnSpeechSateChanged;
        public SpeechMessageReceiveHandler OnSpeechMessageReceived;

        public SpeechCodingProcessor(Setting setting)
        {
            this.Setting = setting;
        }

        public bool CheckPrerequisite()
        {
            if (WaveIn.DeviceCount == 0)
            {
                this.Feedback(MessageType.Warnning, MessageCode.NoAudioInputDevice);
                try
                {
                    ProcessHelper.ExecuteCommands(new string[] { "control mmsys.cpl sounds" });
                }
                catch (Exception ex)
                {
                }
                return false;
            }

            return true;
        }

        public async void Init()
        {
            if (!this.CheckPrerequisite())
            {
                return;
            }

            this.engine = new SpeechRecognitionEngine(new CultureInfo(this.cultureName));
            this.interpreter = LanguageInterpreterHelper.GetInterpreter(this.Language);
            if (this.interpreter != null)
            {
                interpreter.Setting = this.Setting;
                this.RelativeFileExtensions = interpreter.GetRelativeFileExtensions().Concat(this.RelativeFileExtensions).ToList();
            }
            else
            {
                this.Feedback(MessageType.Error, MessageCode.LanguageNotSpecified, null);
            }

            this.engine.SpeechRecognized += Engine_SpeechRecognized;

            this.InitEngine();
            this.InitGrammer();

            if (this.Setting.StartRecordWhenStartup)
            {
                await this.StartRecord();
            }
        }

        private void Engine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string result = e.Result.Text;

            if (this.OnSpeechMessageReceived != null)
            {
                this.OnSpeechMessageReceived(MessageType.Information, MessageCode.Undefined, result);
            }

            if (this.interpreter != null)
            {
                interpreter.Write(result);
            }
        }

        private void InitEngine()
        {
            this.engine.SetInputToDefaultAudioDevice();

            this.engine.InitialSilenceTimeout = TimeSpan.FromSeconds(30);

            this.engine.AudioStateChanged += Engine_AudioStateChanged;
        }

        public void InitGrammer()
        {
            List<string> words = new List<string>();

            var excludeWords = this.GetCustomWords(SettingManager.CustomExcludeWordsFilePath).ToArray();

            var controls = this.GetControlWords();
            var keywords = this.GetKeywords();
            var includeWords = this.GetCustomWords(SettingManager.CustomIncludeWordsFilePath);

            var cleanControls = this.ExcludeWords(controls, excludeWords);
            var cleanKeywords = this.ExcludeWords(keywords, excludeWords);
            var cleanIncludeWords = this.ExcludeWords(includeWords, excludeWords);

            words.AddRange(cleanControls);
            words.AddRange(cleanKeywords);
            words.AddRange(cleanIncludeWords);

            this.LoadGrammer(GrammarType.Control, cleanControls.ToArray());
            this.LoadGrammer(GrammarType.Keyword, cleanKeywords.ToArray());
            this.LoadGrammer(GrammarType.Custom, cleanIncludeWords.ToArray());
        }

        private IEnumerable<string> ExcludeWords(IEnumerable<string> words, string[] includeWords)
        {
            if (includeWords != null && includeWords.Length > 0)
            {
                return words.Except(includeWords);
            }
            return words;
        }

        public void LoadGrammer(GrammarType type, string[] words)
        {
            if (words == null)
            {
                this.Feedback(MessageType.Error, MessageCode.Undefined, "The argument \"words\" is null when call LoadGrammer.");
                return;
            }

            GrammarPriority priority = GrammarPriority.Medium;
            GrammarPrioritySetting gs = this.Setting.GrammarPriorities.FirstOrDefault(item => item.GrammarType == type);

            if (gs != null)
            {
                priority = gs.GrammarPriority;
            }

            string name = type.ToString();

            Grammar grammar = this.engine.Grammars.FirstOrDefault(item => item.Name == name);

            if (grammar != null)
            {
                this.engine.UnloadGrammar(grammar);
            }

            this.dictGrammarWords[type] = words;

            if (words.Length == 0)
            {
                return;
            }

            GrammarBuilder gb = new GrammarBuilder();
            gb.Culture = new CultureInfo(this.cultureName);

            gb.Append(new Choices(words));

            grammar = new Grammar(gb) { Name = name };
            grammar.Priority = (int)priority;
            engine.LoadGrammar(grammar);
        }

        public void FilterGrammar(GrammarType type)
        {
            if (this.engine != null)
            {
                foreach (Grammar grammar in this.engine.Grammars)
                {
                    GrammarType grammarType = (GrammarType)Enum.Parse(typeof(GrammarType), grammar.Name);
                    if (!((grammarType & type) == grammarType))
                    {
                        grammar.Enabled = false;
                    }
                    else
                    {
                        grammar.Enabled = true;
                    }
                }
            }
        }

        private async void Engine_AudioStateChanged(object sender, AudioStateChangedEventArgs e)
        {
            if (e.AudioState == AudioState.Stopped && !this.stoppedManually)
            {
                await this.StartRecord();
            }

            if (this.OnSpeechSateChanged != null)
            {
                this.OnSpeechSateChanged((SpeechState)((int)e.AudioState));
            }
        }

        public async Task StartRecord()
        {
            if (this.engine.AudioState == AudioState.Stopped)
            {
                if (this.OnSpeechMessageReceived != null)
                {
                    this.OnSpeechMessageReceived(MessageType.Information, MessageCode.Undefined, "recording...");
                }

                var task = Task.Factory.StartNew(new Action(() =>
                 {
                     engine.RecognizeAsync(RecognizeMode.Multiple);
                 }));

                await task.ContinueWith(i => { this.stoppedManually = false; });
            }
        }

        public async Task StopRecord()
        {
            if (this.engine != null && this.engine.AudioState != AudioState.Stopped)
            {
                var task = Task.Factory.StartNew(new Action(() =>
                {
                    this.engine.RecognizeAsyncStop();
                }));

                await task.ContinueWith(i => { this.stoppedManually = true; });
            }
        }

        public IEnumerable<string> GetReferenceFilePaths(string filePath)
        {
            List<string> paths = new List<string>();

            FileInfo file = new FileInfo(filePath);
            string extension = file.Extension.ToLower();

            if (this.RelativeFileExtensions.Contains(extension))
            {
                if (extension == ".csproj")
                {
                    CsharpProjectFileParser parser = new CsharpProjectFileParser(filePath, true);
                    CsharpProjectFileInfo info = parser.Parse();
                    paths.AddRange(CsharpProjectFileParser.GetReferencePaths(info));
                }
                else
                {
                    paths.Add(filePath);
                }
            }

            return paths.Distinct();
        }

        public List<ObjectInfo> GetObjectInfos(IEnumerable<string> filePaths)
        {
            List<ObjectInfo> objectInfos = new List<ObjectInfo>();

            foreach (string path in filePaths)
            {
                IEnumerable<string> referenceFilePaths = this.GetReferenceFilePaths(path);
                foreach (string referFilePath in referenceFilePaths)
                {
                    string extension = Path.GetExtension(referFilePath).ToLower();

                    if (extension == ".dll")
                    {
                        if (FileHelper.IsManagedAssembly(referFilePath))
                        {
                            AssemblyParser parser = new AssemblyParser(referFilePath);
                            objectInfos.AddRange(parser.Parse());
                        }
                        else
                        {
                            this.Feedback(MessageType.Warnning, MessageCode.Undefined, $"File \"{referFilePath}\" isn't a managed assembly.");
                        }
                    }
                    else if (extension == ".cs")
                    {
                        CsharpFileParser parser = new CsharpFileParser(referFilePath);
                        CsharpFileInfo info = parser.Parse();
                        objectInfos.AddRange(CsharpFileParser.GetObjectInfos(info));
                    }
                    else if (extension == ".jar")
                    {
                        JarParser parser = new JarParser(referFilePath);
                        objectInfos.AddRange(parser.Parse());
                    }
                }
            }

            return objectInfos;
        }

        public IEnumerable<string> GetPlainTextWords(IEnumerable<string> filePaths)
        {
            List<string> lstWord = new List<string>();
            foreach (string path in filePaths)
            {
                string extension = Path.GetExtension(path).ToLower();

                if (this.RelativeFileExtensions.Contains(extension) && FileHelper.IsTextFile(path) && !this.needParsePlainTextFileExtensions.Contains(extension))
                {
                    string content = File.ReadAllText(path);
                    lstWord.AddRange(this.GetWordsFromString(content));
                }
            }

            return this.FilterWords(lstWord);
        }

        private IEnumerable<string> FilterWords(IEnumerable<string> words)
        {
            Regex reg = new Regex("^[a-zA-Z.]+$");

            return words.Where(item => item.Length >= 2 && reg.IsMatch(item)).Distinct();
        }

        public IEnumerable<string> GetWordsFromString(string content)
        {
            string[] words = content.Split(new char[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return words.Select(item => item.Trim(new char[] { '\r', '\n', '.', ';', ',' }));
        }

        public IEnumerable<string> GetCustomWords(string filePath)
        {
            if (File.Exists(filePath))
            {
                string content = File.ReadAllText(filePath);
                return this.GetWordsFromString(content);
            }

            return Enumerable.Empty<string>();
        }

        public IEnumerable<string> GetWords(List<ObjectInfo> objInfos)
        {
            var names = objInfos.Select(item => item.Name).Distinct();

            return names;
        }

        public IEnumerable<string> GetControlWords()
        {
            var words = this.Setting.ControlWords.Where(item => !string.IsNullOrEmpty(item.Word)).Select(item => item.Word);
            return words;
        }

        public IEnumerable<string> GetKeywords()
        {
            List<string> keywords = new List<string>();

            if (!string.IsNullOrEmpty(this.Setting.PreferredLanguage))
            {
                string filePath = Path.Combine(Utility.CurrentFolder, "Keyword", $"{this.Setting.PreferredLanguage}.txt");
                if (File.Exists(filePath))
                {
                    keywords.AddRange(File.ReadLines(filePath));
                }
            }

            return keywords;
        }

        private void Feedback(MessageType type, MessageCode msgCode, string message = null)
        {
            if (this.OnSpeechMessageReceived != null)
            {
                if (string.IsNullOrEmpty(message))
                {
                    string resourceValue = Resources.ResourceManager.GetString(msgCode.ToString());
                    if (!string.IsNullOrEmpty(resourceValue))
                    {
                        message = resourceValue;
                    }
                }

                this.OnSpeechMessageReceived(type, msgCode, message);
            }
        }

        public async void Dispose()
        {
            if (this.engine != null)
            {
                await this.StopRecord();
                this.engine.SetInputToNull();
                this.engine.RecognizeAsyncCancel();
                this.engine.Dispose();
                this.engine = null;
            }
        }
    }
}
