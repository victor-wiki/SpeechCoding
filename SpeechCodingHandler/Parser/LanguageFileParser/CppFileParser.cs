using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CodeParser;
using System.IO;
using static CodeParser.CPP14Parser;

namespace SpeechCodingHandler
{
    public class CppFileParser : LanguageFileParser
    {
        CppFileInfo cppFileInfo = null;

        public override string FileExtension => ".cpp";

        public CppFileParser() : base() { }

        public CppFileParser(string filePath) : base(filePath) { }

        public override LanguageFileInfo Parse()
        {
            this.cppFileInfo = new CppFileInfo() { FileInfo = new FileInfo(this.FilePath) };

            Lexer lexer = new CPP14Lexer(CharStreams.fromPath(this.FilePath));

            CommonTokenStream tokens = new CommonTokenStream(lexer);

            CPP14Parser parser = new CPP14Parser(tokens);

            ParserRuleContext context = parser.translationunit();

            var children = context.children;

            foreach (IParseTree child in children)
            {
                this.ParseNode(child);
            }

            return this.cppFileInfo;
        }

        private void ParseNode(IParseTree node)
        {
            if (node is DeclarationseqContext)
            {
                DeclarationseqContext declarationseqContext = node as DeclarationseqContext;
                this.ParseDeclarationseq(declarationseqContext);
            }
        }

        private void ParseDeclarationseq(DeclarationseqContext node)
        {
            DeclarationseqContext declareSq = node.declarationseq();
            DeclarationContext declare = node.declaration();

            if (declareSq != null)
            {
                this.ParseDeclarationseq(declareSq);
            }

            if (declare != null)
            {
                AttributedeclarationContext attribute = declare.attributedeclaration();
                FunctiondefinitionContext function = declare.functiondefinition();
                BlockdeclarationContext block = declare.blockdeclaration();

                if (block != null)
                {
                    UsingdirectiveContext usingDirective = block.usingdirective();
                    SimpledeclarationContext simpleDeclaration = block.simpledeclaration();

                    if (usingDirective != null)
                    {
                        string name = usingDirective.namespacename().GetText();

                        this.cppFileInfo.Usings.Add(name);
                    }
                    else if (simpleDeclaration != null)
                    {
                        DeclspecifierseqContext[] decls = simpleDeclaration.GetRuleContexts<DeclspecifierseqContext>();
                        foreach (DeclspecifierseqContext decl in decls)
                        {
                            DeclspecifierContext declSpec = decl.declspecifier();
                            TypespecifierContext typeSpec = declSpec.typespecifier();

                            if (typeSpec != null)
                            {
                                ClassspecifierContext classSpec = typeSpec.classspecifier();
                                EnumspecifierContext enumSpec = typeSpec.enumspecifier();

                                if (classSpec != null)
                                {
                                    string className = classSpec.classhead().classheadname().GetText();

                                    ObjectInfo classObjInfo = new ObjectInfo() { Type = ObjectType.Class, Name = className };
                                    this.cppFileInfo.ObjectInfos.Add(classObjInfo);

                                    MemberspecificationContext[] members = classSpec.GetRuleContexts<MemberspecificationContext>();

                                    foreach (MemberspecificationContext member in members)
                                    {
                                        MemberspecificationContext[] memberSpecs = member.GetRuleContexts<MemberspecificationContext>();
                                        foreach (MemberspecificationContext memberSpec in memberSpecs)
                                        {
                                            MemberdeclarationContext memberDeclaration = memberSpec.memberdeclaration();
                                            MemberspecificationContext memberSpecDeclaration = memberSpec.memberspecification();

                                            this.ParseMemberDeclaration(memberDeclaration, classObjInfo);
                                            this.ParseMemberSpecification(memberSpecDeclaration, classObjInfo);
                                        }
                                    }

                                    this.cppFileInfo.ObjectInfos.AddRange(classObjInfo.Children);
                                }
                            }
                        }
                    }
                }
                else if (function != null)
                {
                    this.ParseFunction(function, null);
                }
            }
        }

        private void ParseFunction(FunctiondefinitionContext node, ObjectInfo parentObjInfo)
        {
            PtrdeclaratorContext[] ptrs = node.declarator().GetRuleContexts<PtrdeclaratorContext>();
            foreach (PtrdeclaratorContext ptr in ptrs)
            {
                NoptrdeclaratorContext noptr = ptr.noptrdeclarator();
                string name = noptr.noptrdeclarator().GetText();

                ObjectInfo methodObjInfo = new ObjectInfo() { Type = ObjectType.Method, Name = name, Parent = parentObjInfo };

                if (parentObjInfo != null)
                {
                    parentObjInfo.Children.Add(methodObjInfo);
                }
                else
                {
                    this.cppFileInfo.ObjectInfos.Add(methodObjInfo);
                }
            }
        }

        private void ParseMemberDeclaration(MemberdeclarationContext node, ObjectInfo parentObjInfo)
        {
            if (node != null)
            {
                MemberspecificationContext[] memberSpecs = node.GetRuleContexts<MemberspecificationContext>();

                MemberdeclaratorlistContext[] fields = node.GetRuleContexts<MemberdeclaratorlistContext>();
                FunctiondefinitionContext[] funcs = node.GetRuleContexts<FunctiondefinitionContext>();

                if (fields != null)
                {
                    foreach (MemberdeclaratorlistContext field in fields)
                    {
                        foreach (string name in field.GetText().Split(','))
                        {
                            ObjectInfo fieldObjInfo = new ObjectInfo() { Type = ObjectType.Field, Name = name, Parent = parentObjInfo };
                            if (parentObjInfo != null)
                            {
                                parentObjInfo.Children.Add(fieldObjInfo);
                            }
                        }
                    }
                }

                if (funcs != null)
                {
                    foreach (FunctiondefinitionContext func in funcs)
                    {
                        this.ParseFunction(func, parentObjInfo);
                    }
                }
            }
        }

        private void ParseMemberSpecification(MemberspecificationContext node, ObjectInfo parentObjInfo)
        {
            if (node != null)
            {
                MemberdeclarationContext[] members = node.GetRuleContexts<MemberdeclarationContext>();
                MemberspecificationContext[] memberSpecs = node.GetRuleContexts<MemberspecificationContext>();

                foreach (MemberdeclarationContext member in members)
                {
                    this.ParseMemberDeclaration(member, parentObjInfo);
                }

                foreach (MemberspecificationContext memberSpec in memberSpecs)
                {
                    this.ParseMemberSpecification(memberSpec, parentObjInfo);
                }
            }
        }
    }
}
