using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SpeechCodingHandler
{
    public class CsharpProjectFileParser
    {
        private readonly string frameworkAssemblyRelativePath = @"Reference Assemblies\Microsoft\Framework\.NETFramework\";

        private string FrameworkAssemblyX86Path
        {
            get { return Path.Combine(FileHelper.SystemDrive, "Program Files (x86)", this.frameworkAssemblyRelativePath); }
        }

        private string FrameworkAssemblyPath
        {
            get { return Path.Combine(FileHelper.SystemDrive, "Program Files", this.frameworkAssemblyRelativePath); }
        }

        public string FilePath { get; set; }
        public bool IncludeSelf { get; set; }
        public CsharpProjectFileParser(string filePath, bool includeSelf = false)
        {
            this.FilePath = filePath;
            this.IncludeSelf = includeSelf;
        }

        public CsharpProjectFileInfo Parse()
        {
            CsharpProjectFileInfo info = new CsharpProjectFileInfo();
            info.FileInfo = new FileInfo(this.FilePath);

            XDocument doc = XDocument.Load(this.FilePath);

            var elements = doc.Root.Elements().Where(item => item.Name.LocalName == "PropertyGroup");

            var basicElement = elements.FirstOrDefault(item => item.Elements().Any(t => t.Name.LocalName == "Configuration"));

            if (basicElement != null)
            {
                info.OutputType = this.GetChildElementValue(basicElement, "OutputType");
                info.Configuration = this.GetChildElementValue(basicElement, "Configuration");
                info.TargetFrameworkVersion = this.GetChildElementValue(basicElement, "TargetFrameworkVersion");
                info.AssemblyName = this.GetChildElementValue(basicElement, "AssemblyName");

                var configurationElement = elements.FirstOrDefault(item => item.Attribute("Condition") != null && item.Attribute("Condition").Value.Contains(info.Configuration));

                if (configurationElement != null)
                {
                    info.PlatformTarget = this.GetChildElementValue(configurationElement, "PlatformTarget");
                    info.OutputPath = this.GetChildElementValue(configurationElement, "OutputPath");
                }
            }

            var refRootElement = doc.Root.Elements().Where(item => item.Name.LocalName == "ItemGroup" && item.Elements().Any(t => t.Name.LocalName == "Reference")).FirstOrDefault();

            if (refRootElement != null)
            {
                foreach (var refElement in refRootElement.Elements())
                {
                    var path = this.GetChildElementValue(refElement, "HintPath");
                    if (string.IsNullOrEmpty(path))
                    {
                        path = refElement.Attribute("Include")?.Value;
                    }

                    if (!string.IsNullOrEmpty(path))
                    {
                        bool isGlobal = false;
                        if (path.StartsWith("..\\"))
                        {
                            path = Path.Combine(new FileInfo(this.FilePath).Directory.Parent.FullName, path.Replace("..\\", ""));
                        }
                        else if (!File.Exists(path))
                        {
                            string assemblyPath = "";

                            if (info.PlatformTarget == "x64")
                            {
                                assemblyPath = Path.Combine(this.FrameworkAssemblyPath, info.TargetFrameworkVersion, path + ".dll");
                            }

                            if (!File.Exists(assemblyPath))
                            {
                                assemblyPath = Path.Combine(this.FrameworkAssemblyX86Path, info.TargetFrameworkVersion, path + ".dll");
                            }

                            path = assemblyPath;
                            isGlobal = true;
                        }

                        if (File.Exists(path))
                        {
                            info.References.Add(new AssemblyReferenceInfo() { Name = Path.GetFileNameWithoutExtension(path), Path = path, IsGlobal = isGlobal });
                        }
                    }
                }
            }

            var compileRootElement = doc.Root.Elements().Where(item => item.Name.LocalName == "ItemGroup" && item.Elements().Any(t => t.Name.LocalName == "Compile")).FirstOrDefault();

            if (compileRootElement != null)
            {
                foreach (var compileElement in compileRootElement.Elements())
                {
                    var path = compileElement.Attribute("Include").Value;
                    string extension = Path.GetExtension(path);
                    if (extension == ".cs")
                    {
                        if (File.Exists(path))
                        {
                            info.CsFilePaths.Add(path);
                        }
                        else
                        {
                            path = Path.Combine(new FileInfo(this.FilePath).DirectoryName, path);
                            info.CsFilePaths.Add(path);
                        }
                    }
                }
            }

            if (this.IncludeSelf)
            {
                string outputFilePath = Path.Combine(info.FileInfo.DirectoryName, info.OutputPath, info.AssemblyName + ".dll");

                if (File.Exists(outputFilePath))
                {
                    info.References.Add(new AssemblyReferenceInfo() { Name = info.AssemblyName, Path = outputFilePath, IsGlobal = false });
                }
            }

            return info;
        }

        private string GetChildElementValue(XElement element, string childName)
        {
            return element.Elements().FirstOrDefault(item => item.Name.LocalName == childName)?.Value;
        }

        public static List<string> GetReferencePaths(CsharpProjectFileInfo info)
        {
            List<string> paths = new List<string>();

            paths.AddRange(info.References.Where(item => item.IsGlobal).Select(item => item.Path));
            paths.AddRange(info.References.Where(item => !item.IsGlobal).Select(item => item.Path));

            return paths;
        }
    }
}
