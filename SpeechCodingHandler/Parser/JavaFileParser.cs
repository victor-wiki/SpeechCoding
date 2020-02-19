using japa.parser;
using japa.parser.ast;
using japa.parser.ast.body;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SpeechCodingHandler
{
    public class JavaFileParser
    {
        private string packageName;

        public string FilePath { get; set; }

        public JavaFileParser(string filePath)
        {
            this.FilePath = filePath;
        }

        public JavaFileInfo Parse()
        {
            JavaFileInfo javaFileInfo = new JavaFileInfo() { FileInfo = new FileInfo(this.FilePath) };

            List<ObjectInfo> objectInfos = new List<ObjectInfo>();

            CompilationUnit result = JavaParser.parse(new java.io.File(this.FilePath));

            var imports = result.getImports().toArray();
            foreach (var import in imports)
            {
                ImportDeclaration importDeclaration = import as ImportDeclaration;
                string importName = importDeclaration.getName().toString();
                javaFileInfo.Imports.Add(importName);
            }

            ObjectInfo packageObjInfo = null;

            PackageDeclaration packageDeclaration = result.getPackage();
            if (packageDeclaration != null)
            {
                this.packageName = packageDeclaration.getName().toString();

                javaFileInfo.PackageName = packageName;

                packageObjInfo = new ObjectInfo() { Type = ObjectType.Namespace, Name = this.packageName };
                objectInfos.Add(packageObjInfo);
            }

            var types = result.getTypes().toArray();

            foreach (var type in types)
            {
                this.ParseNode(type as Node, objectInfos, packageObjInfo);
            }

            javaFileInfo.ObjectInfos = objectInfos;

            return javaFileInfo;
        }

        private void ParseNode(Node node, List<ObjectInfo> objectInfos, ObjectInfo packageObjInfo)
        {
            if (node is ClassOrInterfaceDeclaration)
            {
                ClassOrInterfaceDeclaration classDeclaration = node as ClassOrInterfaceDeclaration;
                ObjectInfo classObjInfo = this.ParseClassNode(classDeclaration);
                this.AddClassObjectInfo(objectInfos, packageObjInfo, classObjInfo);
            }
            else if (node is EnumDeclaration)
            {
                EnumDeclaration enumDeclaration = node as EnumDeclaration;
                string name = enumDeclaration.getName();
                var enumObjInfo = new ObjectInfo() { Name = name, Type = ObjectType.Enum, Parent = packageObjInfo, Assembly = this.packageName };
                if (enumObjInfo != null)
                {
                    packageObjInfo.Children.Add(enumObjInfo);
                }
                objectInfos.Add(enumObjInfo);
            }
        }

        private void AddClassObjectInfo(List<ObjectInfo> objectInfos, ObjectInfo packageObjInfo, ObjectInfo classObjInfo)
        {
            if (packageObjInfo != null)
            {
                packageObjInfo.Children.Add(classObjInfo);
                classObjInfo.Parent = packageObjInfo;
                classObjInfo.Assembly = this.packageName;
            }

            objectInfos.Add(classObjInfo);
            objectInfos.AddRange(classObjInfo.Children);
        }

        private ObjectInfo ParseClassNode(ClassOrInterfaceDeclaration node)
        {
            string className = node.getName();
            bool isClass = !node.isInterface();

            ObjectInfo objectInfo = new ObjectInfo() { Type = isClass ? ObjectType.Class : ObjectType.Interface, Name = className };

            var members = node.getMembers().toArray();
            foreach (var member in members)
            {
                if (member is FieldDeclaration)
                {
                    FieldDeclaration fieldDeclaration = member as FieldDeclaration;
                    string name = "";
                    foreach (var field in fieldDeclaration.getVariables().toArray())
                    {
                        VariableDeclarator variable = field as VariableDeclarator;
                        var children = variable.getChildrenNodes().toArray();
                        if (children.Length > 0)
                        {
                            name = variable.getChildrenNodes().get(0).ToString();
                            break;
                        }
                    }

                    if (!string.IsNullOrEmpty(name))
                    {
                        var fieldObj = new ObjectInfo() { Name = name, Type = ObjectType.Field, Parent = objectInfo };
                        objectInfo.Children.Add(fieldObj);
                    }
                }
                else if (member is MethodDeclaration)
                {
                    MethodDeclaration methodDeclaration = member as MethodDeclaration;
                    var methodObj = new ObjectInfo() { Name = methodDeclaration.getName(), Type = ObjectType.Method, Parent = objectInfo };
                    objectInfo.Children.Add(methodObj);
                }
            }

            return objectInfo;
        }
    }
}
