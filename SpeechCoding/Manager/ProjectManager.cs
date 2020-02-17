using Newtonsoft.Json;
using System.IO;
using SpeechCodingHandler;

namespace SpeechCoding
{
    public class ProjectManager
    {
        public static void Init()
        {
            if(!Directory.Exists(DefaultSaveFolder))
            {
                Directory.CreateDirectory(DefaultSaveFolder);
            }
        }

        public static string DefaultSaveFolder
        {
            get
            {
                return Path.Combine(Utility.CurrentFolder, "Project");
            }
        }
        
        public static ProjectInfo GetProjectInfo(string filePath)
        {
            if (File.Exists(filePath))
            {
                string content = File.ReadAllText(filePath);
                ProjectInfo recentInfo = (ProjectInfo)JsonConvert.DeserializeObject(content, typeof(ProjectInfo));
                return recentInfo;
            }
            return null;
        }

        public static void SaveProject(ProjectInfo info, string filePath)
        {            
            string content = JsonConvert.SerializeObject(info, Formatting.Indented);
            File.WriteAllText(filePath, content);           
        }        
    }
}
