using System.Collections.Generic;
using System.Diagnostics;

namespace SpeechCodingHandler
{
    public class ProcessHelper
    {
        public static void ExecuteCommands(IEnumerable<string> commands)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";         
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();

            foreach(string command in commands)
            {
                p.StandardInput.WriteLine(command);
            }          

            p.StandardInput.AutoFlush = true;
            p.StandardInput.Flush();
            p.StandardInput.Close();
            p.WaitForExit();
            p.Close();
            p.Dispose();
        }
    }
}
