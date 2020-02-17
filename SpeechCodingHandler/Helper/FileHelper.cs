using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace SpeechCodingHandler
{
    public class FileHelper
    {
        public static string SystemDrive = Path.GetPathRoot(Environment.SystemDirectory);

        public static bool IsTextFile(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                int character;
                while ((character = reader.Read()) != -1)
                {
                    if ((character > 0 && character < 8) || (character > 13 && character < 26))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool IsManagedAssembly(string filePath)
        {
            try
            {
                Assembly.LoadFrom(filePath);
                return true;
            }
            catch (BadImageFormatException ex)
            {
                return false;
            }
        }
    }
}
