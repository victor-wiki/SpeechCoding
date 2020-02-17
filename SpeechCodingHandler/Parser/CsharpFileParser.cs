using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SpeechCodingHandler
{
    public class CsharpFileParser
    {
        private string assemblyName;
        public string FilePath { get; set; }
        public bool IncludeNamespace { get; set; }
        public CsharpFileParser(string filePath, bool includeNamespace = false)
        {
            this.FilePath = filePath;
            this.IncludeNamespace = includeNamespace;
        }

        public CsharpFileInfo Parse()
        {
            CsharpFileInfo csharpFileInfo = new CsharpFileInfo();
            csharpFileInfo.FileInfo = new FileInfo(this.FilePath);

            CsharpProjectFileInfo proFileInfo = this.FindProjectFile(new FileInfo(this.FilePath).Directory);
            csharpFileInfo.ProjectInfo = proFileInfo;

            this.assemblyName = proFileInfo?.AssemblyName;
            csharpFileInfo.AssemblyName = this.assemblyName;

            string content = File.ReadAllText(this.FilePath);
            SyntaxTree tree = CSharpSyntaxTree.ParseText(content, null, null);
            SyntaxNode root = tree.GetRoot();

            IEnumerable<SyntaxNode> nodes = root.ChildNodes();

            ObjectInfo namespaceObjInfo = null;
            List<ObjectInfo> objectInfos = new List<ObjectInfo>();

            foreach (SyntaxNode node in nodes)
            {
                if (node is UsingDirectiveSyntax)
                {
                    UsingDirectiveSyntax usingNode = node as UsingDirectiveSyntax;
                    string ns = usingNode.Name.GetText().ToString();

                    csharpFileInfo.Usings.Add(ns);
                }
                else if (node is NamespaceDeclarationSyntax)
                {
                    NamespaceDeclarationSyntax nsNode = node as NamespaceDeclarationSyntax;
                    string nsName = nsNode.Name.GetText().ToString().Trim();
                    csharpFileInfo.Namespace = nsName;

                    if (this.IncludeNamespace)
                    {
                        namespaceObjInfo = new ObjectInfo() { Name = nsName, Type = ObjectType.Namespace, Assembly = this.assemblyName };
                        objectInfos.Add(namespaceObjInfo);
                    }

                    IEnumerable<SyntaxNode> nsChildNodes = nsNode.Members;

                    foreach (var nsChildNode in nsChildNodes)
                    {
                        this.ParseNode(nsChildNode, objectInfos, namespaceObjInfo);
                    }
                }
                else
                {
                    this.ParseNode(node, objectInfos, namespaceObjInfo);
                }
            }

            csharpFileInfo.ObjectInfos = objectInfos;

            return csharpFileInfo;
        }

        private void ParseNode(SyntaxNode node, List<ObjectInfo> objectInfos, ObjectInfo namespaceObjInfo)
        {
            if (node is ClassDeclarationSyntax)
            {
                ClassDeclarationSyntax classNode = node as ClassDeclarationSyntax;
                ObjectInfo classObjInfo = this.ParseClassNode(classNode);
                this.AddClassObjectInfo(objectInfos, namespaceObjInfo, classObjInfo);
            }
            else if (node is DelegateDeclarationSyntax)
            {
                DelegateDeclarationSyntax delegateNode = node as DelegateDeclarationSyntax;
                string name = delegateNode.Identifier.ValueText;
                var delegateObjInfo = new ObjectInfo() { Name = name, Type = ObjectType.Delegate, Parent = namespaceObjInfo, Assembly = this.assemblyName };
                if (namespaceObjInfo != null)
                {
                    namespaceObjInfo.Children.Add(delegateObjInfo);
                }
                objectInfos.Add(delegateObjInfo);
            }
        }

        private void AddClassObjectInfo(List<ObjectInfo> objectInfos, ObjectInfo namespaceObjInfo, ObjectInfo classObjInfo)
        {
            if (namespaceObjInfo != null)
            {
                namespaceObjInfo.Children.Add(classObjInfo);
                classObjInfo.Parent = namespaceObjInfo;
            }

            objectInfos.Add(classObjInfo);
            objectInfos.AddRange(classObjInfo.Children);
        }

        private ObjectInfo ParseClassNode(ClassDeclarationSyntax node)
        {
            string className = node.Identifier.Value.ToString();

            var objectInfo = new ObjectInfo() { Name = className, Type = ObjectType.Class, Parent = null, Assembly = null };

            IEnumerable<SyntaxNode> classChildNodes = node.Members;

            foreach (var classChildNode in classChildNodes)
            {
                if (classChildNode is FieldDeclarationSyntax)
                {
                    FieldDeclarationSyntax fieldNode = classChildNode as FieldDeclarationSyntax;
                    string name = fieldNode.Declaration.Variables.FirstOrDefault()?.Identifier.ValueText;
                    if (!string.IsNullOrEmpty(name))
                    {
                        var fieldObj = new ObjectInfo() { Name = name, Type = ObjectType.Field, Parent = objectInfo };
                        objectInfo.Children.Add(fieldObj);
                    }
                }
                else if (classChildNode is PropertyDeclarationSyntax)
                {
                    PropertyDeclarationSyntax propertyNode = classChildNode as PropertyDeclarationSyntax;

                    var propertyObj = new ObjectInfo() { Name = propertyNode.Identifier.ValueText, Type = ObjectType.Property, Parent = objectInfo };
                    objectInfo.Children.Add(propertyObj);
                }
                else if (classChildNode is MethodDeclarationSyntax)
                {
                    MethodDeclarationSyntax methodNode = classChildNode as MethodDeclarationSyntax;
                    var methodObj = new ObjectInfo() { Name = methodNode.Identifier.ValueText, Type = ObjectType.Method, Parent = objectInfo };
                    objectInfo.Children.Add(methodObj);
                }
                else if (classChildNode is EventDeclarationSyntax)
                {
                    EventDeclarationSyntax eventNode = classChildNode as EventDeclarationSyntax;
                    var eventObj = new ObjectInfo() { Name = eventNode.Identifier.ValueText, Type = ObjectType.Event, Parent = objectInfo };
                    objectInfo.Children.Add(eventObj);
                }
                else if (classChildNode is DelegateDeclarationSyntax)
                {
                    DelegateDeclarationSyntax delegateNode = classChildNode as DelegateDeclarationSyntax;
                    var delegateObj = new ObjectInfo() { Name = delegateNode.Identifier.ValueText, Type = ObjectType.Event, Parent = objectInfo };
                    objectInfo.Children.Add(delegateObj);
                }
            }

            return objectInfo;
        }

        private CsharpProjectFileInfo FindProjectFile(DirectoryInfo folder)
        {
            var projectFiles = folder.GetFiles("*.csproj");
            if (projectFiles.Length > 0)
            {
                foreach (var projFile in projectFiles)
                {
                    CsharpProjectFileParser parser = new CsharpProjectFileParser(projFile.FullName, true);
                    CsharpProjectFileInfo projFileInfo = parser.Parse();
                    if (projFileInfo.CsFilePaths.Contains(this.FilePath))
                    {
                        return projFileInfo;
                    }
                }
            }
            else
            {
                return this.FindProjectFile(folder.Parent);
            }

            return null;
        }

        public static List<ObjectInfo> GetObjectInfos(CsharpFileInfo info)
        {
            List<ObjectInfo> objectInfos = new List<ObjectInfo>();

            objectInfos.AddRange(info.ObjectInfos);

            if (info.ProjectInfo != null)
            {
                foreach (AssemblyReferenceInfo referInfo in info.ProjectInfo.References)
                {
                    AssemblyParser assemblyParser = new AssemblyParser(referInfo.Path);
                    List<ObjectInfo> assemblyObjInfos = assemblyParser.Parse();
                    var matchedNamespaceObjInfos = assemblyObjInfos.Where(item => item.Type == ObjectType.Namespace && info.Usings.Contains(item.Name) || info.AssemblyName == item.Name);

                    if (matchedNamespaceObjInfos.Count() > 0)
                    {
                        foreach (var nsObjInfo in matchedNamespaceObjInfos)
                        {
                            objectInfos.AddRange(ObjectInfoHelper.GetChildlren(nsObjInfo));
                        }
                    }
                }
            }

            return objectInfos;
        }
    }
}
