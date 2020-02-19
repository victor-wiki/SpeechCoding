using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;

namespace SpeechCodingHandler
{
    public class JarParser
    {
        public string FilePath { get; set; }

        public JarParser(string path)
        {
            this.FilePath = path;
        }

        public List<ObjectInfo> Parse()
        {
            List<ObjectInfo> objectInfos = new List<ObjectInfo>();

            string assemblyName = Path.GetFileNameWithoutExtension(this.FilePath);
            string strNamespace = "";

            List<string> classNames = new List<string>();

            using (var fs = new FileStream(this.FilePath, FileMode.Open, FileAccess.Read))
            {
                using (ZipFile zf = new ZipFile(fs))
                {
                    foreach (ZipEntry ze in zf)
                    {
                        if (ze.IsFile)
                        {
                            string name = ze.Name;
                            string extension = Path.GetExtension(name);
                            if (extension == ".class")
                            {
                                strNamespace = Path.GetDirectoryName(name).Replace("\\", ".");
                                string fullName = strNamespace + "." + Path.GetFileNameWithoutExtension(name);
                                classNames.Add(fullName);
                            }
                        }
                    }
                }
            }

            java.net.URL url = new java.net.URL($"file:{this.FilePath}");
            java.net.URL[] urls = { url };

            java.net.URLClassLoader loader = new java.net.URLClassLoader(urls);
            try
            {
                ObjectInfo packageObjInfo = new ObjectInfo() { Name = strNamespace, Type = ObjectType.Namespace, Assembly = assemblyName };
                objectInfos.Add(packageObjInfo);

                List<Type> types = new List<Type>();

                foreach(string className in classNames)
                {
                    java.lang.Class cl = java.lang.Class.forName(className, false, loader);
                    int modifiers = cl.getModifiers();

                    if((modifiers & java.lang.reflect.Modifier.PRIVATE)== java.lang.reflect.Modifier.PRIVATE)
                    {
                        continue;
                    }

                    ObjectType objType = ObjectType.Unknown;
                    if(cl.isInterface())
                    {
                        objType = ObjectType.Interface;
                    }
                    else if(cl.isEnum())
                    {
                        objType = ObjectType.Enum;
                    }
                    else
                    {
                        objType = ObjectType.Class;
                    }

                    var objectInfo = new ObjectInfo() { Name = cl.getName(), Type = objType, Parent = packageObjInfo, Assembly = assemblyName };
                    objectInfos.Add(objectInfo);
                    packageObjInfo.Children.Add(objectInfo);

                    foreach (var field in cl.getFields())
                    {
                        string name = field.getName();
                        var fieldObj = new ObjectInfo() { Name = name, Type = ObjectType.Field, Parent = objectInfo };
                        objectInfos.Add(fieldObj);
                    }

                    foreach(var method in cl.getMethods())
                    {
                        string name = method.getName();
                        var methodObj = new ObjectInfo() { Name = name, Type = ObjectType.Method, Parent = objectInfo };
                        objectInfos.Add(methodObj);
                        objectInfo.Children.Add(methodObj);
                    }
                }               
            }
            catch (Exception ex)
            {
            }

            return objectInfos;
        }
    }
}
