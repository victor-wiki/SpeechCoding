using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CodeParser;
using System.IO;
using static CodeParser.PhpParser;

namespace SpeechCodingHandler
{
    public class PhpFileParser : LanguageFileParser
    {
        private PhpFileInfo phpFileInfo = null;

        public override string FileExtension => ".php";

        public PhpFileParser() : base() { }

        public PhpFileParser(string filePath) : base(filePath) { }

        public override LanguageFileInfo Parse()
        {
            this.phpFileInfo = new PhpFileInfo() { FileInfo = new FileInfo(this.FilePath) };

            Lexer lexer = new PhpLexer(CharStreams.fromPath(this.FilePath));

            CommonTokenStream tokens = new CommonTokenStream(lexer);

            PhpParser parser = new PhpParser(tokens);

            ParserRuleContext context = parser.htmlDocument();

            var children = context.children;

            foreach (IParseTree child in children)
            {
                this.ParseNode(child);
            }

            return phpFileInfo;
        }

        private void ParseNode(IParseTree node)
        {
            if (node is HtmlElementOrPhpBlockContext)
            {
                HtmlElementOrPhpBlockContext block = node as HtmlElementOrPhpBlockContext;

                PhpBlockContext phpBlock = block.phpBlock();

                foreach (TopStatementContext topStatement in phpBlock.topStatement())
                {
                    UseDeclarationContext useDeclaration = topStatement.useDeclaration();
                    StatementContext statement = topStatement.statement();
                    ClassDeclarationContext classDeclaration = topStatement.classDeclaration();
                    FunctionDeclarationContext function = topStatement.functionDeclaration();

                    if (useDeclaration != null)
                    {
                        string name = useDeclaration.useDeclarationContentList().GetText();
                        this.phpFileInfo.Usings.Add(name);
                    }
                    else if (statement != null)
                    {
                        ExpressionStatementContext expression = statement.expressionStatement();

                        if (expression != null)
                        {
                            AssignmentExpressionContext[] assignments = expression.GetRuleContexts<AssignmentExpressionContext>();
                            ScalarExpressionContext[] scalarExpressions = expression.GetRuleContexts<ScalarExpressionContext>();

                            foreach (AssignmentExpressionContext assignment in assignments)
                            {
                                string name = assignment.assignable().GetText();
                                ObjectInfo fieldObjInfo = new ObjectInfo() { Type = ObjectType.Field, Name = name };
                                this.phpFileInfo.ObjectInfos.Add(fieldObjInfo);
                            }

                            foreach (ScalarExpressionContext scalarExpression in scalarExpressions)
                            {
                                string nsName = scalarExpression?.constant()?.qualifiedNamespaceName()?.namespaceNameList()?.GetText();

                                if (!string.IsNullOrEmpty(nsName))
                                {
                                    this.phpFileInfo.Namespace = nsName;
                                }
                            }
                        }
                    }
                    else if (classDeclaration != null)
                    {
                        string className = classDeclaration.identifier().GetText();

                        ObjectInfo classObjInfo = new ObjectInfo() { Type = ObjectType.Class, Name = className };
                        this.phpFileInfo.ObjectInfos.Add(classObjInfo);

                        ClassStatementContext[] classStatements = classDeclaration.GetRuleContexts<ClassStatementContext>();

                        foreach (ClassStatementContext classStatement in classStatements)
                        {
                            VariableInitializerContext[] variables = classStatement.variableInitializer();
                            bool isField = variables.Length > 0;

                            foreach (VariableInitializerContext variable in variables)
                            {
                                string name = variable.GetText();
                                ObjectInfo fieldObjInfo = new ObjectInfo() { Type = ObjectType.Field, Name = name, Parent = classObjInfo };
                                classObjInfo.Children.Add(fieldObjInfo);
                            }

                            if (!isField)
                            {
                                IdentifierContext[] identifiers = classStatement.GetRuleContexts<IdentifierContext>();

                                foreach (IdentifierContext identifier in identifiers)
                                {
                                    string name = identifier.GetText();
                                    ObjectInfo methodObjInfo = new ObjectInfo() { Type = ObjectType.Method, Name = name, Parent = classObjInfo };
                                    classObjInfo.Children.Add(methodObjInfo);
                                }
                            }
                        }

                        this.phpFileInfo.ObjectInfos.AddRange(classObjInfo.Children);
                    }
                    else if (function != null)
                    {
                        string name = function.identifier().GetText();

                        ObjectInfo methodObjInfo = new ObjectInfo() { Type = ObjectType.Method, Name = name };
                        this.phpFileInfo.ObjectInfos.Add(methodObjInfo);
                    }
                }
            }
        }
    }
}
