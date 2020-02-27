using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CodeParser;
using System.IO;
using static CodeParser.CParser;

namespace SpeechCodingHandler
{
    public class CFileParser : LanguageFileParser
    {
        private CFileInfo cFileInfo = null;

        public override string FileExtension => ".c";

        public CFileParser() : base() { }

        public CFileParser(string filePath) : base(filePath) { }


        public override LanguageFileInfo Parse()
        {
            this.cFileInfo = new CFileInfo() { FileInfo = new FileInfo(this.FilePath) };

            Lexer lexer = new CLexer(CharStreams.fromPath(this.FilePath));

            CommonTokenStream tokens = new CommonTokenStream(lexer);

            CParser parser = new CParser(tokens);

            ParserRuleContext context = parser.compilationUnit();

            var children = context.children;

            foreach (IParseTree child in children)
            {
                this.ParseNode(child);
            }

            return this.cFileInfo;
        }

        private void ParseNode(IParseTree node)
        {
            if (node is TranslationUnitContext)
            {
                TranslationUnitContext transUnit = node as TranslationUnitContext;
                this.ParseTranslationUnit(transUnit);
            }
        }

        private void ParseTranslationUnit(TranslationUnitContext node)
        {
            TranslationUnitContext transUnit = node.translationUnit();

            ExternalDeclarationContext externalDeclaration = node.externalDeclaration();

            if (transUnit != null)
            {
                this.ParseTranslationUnit(transUnit);
            }

            if (externalDeclaration != null)
            {
                DeclarationContext[] declarations = externalDeclaration.GetRuleContexts<DeclarationContext>();
                FunctionDefinitionContext[] functions = externalDeclaration.GetRuleContexts<FunctionDefinitionContext>();

                foreach (DeclarationContext declaration in declarations)
                {
                    InitDeclaratorListContext initDeclaratorList = declaration.initDeclaratorList();
                    string name = initDeclaratorList.initDeclarator().declarator().directDeclarator().Identifier().GetText();

                    ObjectInfo fieldObjInfo = new ObjectInfo() { Type = ObjectType.Field, Name = name };
                    this.cFileInfo.ObjectInfos.Add(fieldObjInfo);
                }

                foreach (FunctionDefinitionContext function in functions)
                {
                    DirectDeclaratorContext[] directDeclarators = function.declarator().GetRuleContexts<DirectDeclaratorContext>();

                    foreach (DirectDeclaratorContext directDeclarator in directDeclarators)
                    {
                        string name = directDeclarator.directDeclarator().Identifier().GetText();

                        ObjectInfo methodObjInfo = new ObjectInfo() { Type = ObjectType.Method, Name = name };
                        this.cFileInfo.ObjectInfos.Add(methodObjInfo);
                    }
                }
            }
        }
    }
}
