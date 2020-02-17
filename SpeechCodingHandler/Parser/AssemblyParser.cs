using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace SpeechCodingHandler
{
    public class AssemblyParser
    {
        public string FilePath { get; set; }

        public AssemblyParser(string path)
        {
            this.FilePath = path;
        }

        public List<ObjectInfo> Parse()
        {
            List<ObjectInfo> objectInfos = new List<ObjectInfo>();

            if (File.Exists(this.FilePath))
            {
                string assemblyName = Path.GetFileName(this.FilePath);

                var assembly = Assembly.LoadFrom(this.FilePath);

                try
                {
                    var types = assembly.GetTypes();

                    objectInfos = TypeHelper.GetObjetInfos(types, assemblyName);
                }
                catch (Exception ex)
                {
                    
                }
            }

            return objectInfos;
        }
    }
}
