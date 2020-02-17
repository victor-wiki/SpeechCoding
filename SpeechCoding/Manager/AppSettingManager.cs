using Newtonsoft.Json;
using System.IO;

namespace SpeechCoding
{
    public class AppSettingManager
    {        
        private const string settingFileName = "AppSetting.json";
        private static AppSetting _setting;            

        public static AppSetting Setting
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

        public static AppSetting GetSetting()
        {
            if (File.Exists(settingFileName))
            {
                string content = File.ReadAllText(settingFileName);
                AppSetting setting = (AppSetting)JsonConvert.DeserializeObject(content, typeof(AppSetting));
                return setting;
            }
            else
            {
                return new AppSetting();
            }
        }

        public static void SaveSetting(AppSetting setting)
        {
            _setting = setting;
            string content = JsonConvert.SerializeObject(setting, Formatting.Indented);
            File.WriteAllText(settingFileName, content);
        }
    }
}
