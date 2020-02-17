using Newtonsoft.Json;
using System.IO;

namespace SpeechCodingHandler
{
    public class ConfigManager
    {
        private const string configFileName = "config.json";
        private static Config _config;

        public static Config Config
        {
            get
            {
                if(_config==null)
                {
                    return GetConfig();
                }
                return _config;
            }

        }

        public static Config GetConfig()
        {
            if(File.Exists(configFileName))
            {
                string content = File.ReadAllText(configFileName);
                Config config = (Config) JsonConvert.DeserializeObject(content, typeof(Config));
                return config;
            }
            else
            {
                return new Config();
            }
        }

        public static void SaveConfig(Config config)
        {
            _config = config;
            string content = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(configFileName, content);
        }
    }
}
