using Newtonsoft.Json;
using System.IO;

namespace SpeechCodingHandler
{
    public class SettingManager
    {        
        private const string settingFileName = "setting.json";
        private static Setting _setting;

        public static string CustomWordFolder { get; set; } = "CustomWords";
        public static string CustomIncludeWordsFilePath  => Path.Combine(CustomWordFolder, "IncludeWords.txt");
        public static string CustomExcludeWordsFilePath  => Path.Combine(CustomWordFolder, "ExcludeWords.txt");       

        public static Setting Setting
        {
            get
            {
                if (_setting == null)
                {
                    return GetSetting();
                }
                return _setting;
            }
        }

        public static bool IsSetted()
        {
            return File.Exists(settingFileName);
        }

        public static Setting GetSetting()
        {
            if (File.Exists(settingFileName))
            {
                string content = File.ReadAllText(settingFileName);
                Setting setting = (Setting)JsonConvert.DeserializeObject(content, typeof(Setting));
                return setting;
            }
            else
            {
                return new Setting();
            }
        }

        public static void SaveSetting(Setting setting)
        {
            _setting = setting;
            string content = JsonConvert.SerializeObject(setting, Formatting.Indented);
            File.WriteAllText(settingFileName, content);
        }
    }
}
