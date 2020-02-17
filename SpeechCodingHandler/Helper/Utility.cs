using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace SpeechCodingHandler
{
    public class Utility
    {
        public static readonly string CurrentFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
               

        public static void OpenInExplorer(string filePath)
        {
            string cmd = "explorer.exe";
            string arg = "/select," + filePath;
            Process.Start(cmd, arg);
        }       
    }
}
