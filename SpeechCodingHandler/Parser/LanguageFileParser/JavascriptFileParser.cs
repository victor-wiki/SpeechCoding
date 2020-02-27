using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CodeParser;
using System.IO;
using System.Linq;
using static CodeParser.JavaScriptParser;

namespace SpeechCodingHandler
{
    public class JavascriptFileParser : LanguageFileParser
    {
        private JavascriptFileInfo jsFileInfo = null;

        public override string FileExtension => ".js";

        public JavascriptFileParser() : base() { }

        public JavascriptFileParser(string filePath) : base(filePath) { }

        public override LanguageFileInfo Parse()
        {
            this.jsFileInfo = new JavascriptFileInfo() { FileInfo = new FileInfo(this.FilePath) };

            Lexer lexer = new JavaScriptLexer(CharStreams.fromPath(this.FilePath));

            CommonTokenStream tokens = new CommonTokenStream(lexer);

            JavaScriptParser parser = new JavaScriptParser(tokens);

            ParserRuleContext context = parser.program();

            var children = context.children;

            foreach (IParseTree child in children)
            {
                this.ParseNode(child);
            }

            return this.jsFileInfo;
        }

        private void ParseNode(IParseTree node)
        {
            if (node is SourceElementsContext)
            {
                SourceElementsContext elementsContext = node as SourceElementsContext;
                SourceElementContext[] srcElements = elementsContext.sourceElement();
                foreach (SourceElementContext srcElement in srcElements)
                {
                    StatementContext statementContext = srcElement.statement();
                    VariableStatementContext varStatementContext = statementContext.variableStatement();
                    FunctionDeclarationContext functionContext = statementContext.functionDeclaration();
                    ImportStatementContext importContext = statementContext.importStatement();
                    ExportStatementContext exportContext = statementContext.exportStatement();

                    if (varStatementContext != null)
                    {
                        VariableDeclarationListContext variableListContex = varStatementContext.variableDeclarationList();

                        if (variableListContex != null)
                        {
                            var variables = variableListContex.children;

                            foreach (var variable in variables)
                            {
                                if (variable is VariableDeclarationContext)
                                {
                                    VariableDeclarationContext declarationContext = variable as VariableDeclarationContext;
                                    string name = declarationContext.assignable().GetText();

                                    ObjectLiteralExpressionContext[] exps = declarationContext.GetRuleContexts<ObjectLiteralExpressionContext>();

                                    if (exps == null || exps.Length == 0)
                                    {
                                        ObjectInfo fieldObjInfo = new ObjectInfo() { Type = ObjectType.Field, Name = name };
                                        this.jsFileInfo.ObjectInfos.Add(fieldObjInfo);
                                    }
                                    else
                                    {
                                        ObjectInfo classObjInfo = new ObjectInfo() { Type = ObjectType.Class, Name = name };
                                        this.jsFileInfo.ObjectInfos.Add(classObjInfo);

                                        foreach (ObjectLiteralExpressionContext exp in exps)
                                        {
                                            ObjectLiteralContext literalContext = exp.objectLiteral();
                                            PropertyAssignmentContext[] properties = literalContext.propertyAssignment();

                                            foreach (PropertyAssignmentContext property in properties)
                                            {
                                                PropertyNameContext propertyNameContext = property.GetRuleContext<PropertyNameContext>(0);
                                                FunctionExpressionContext funcExpContext = property.GetRuleContext<FunctionExpressionContext>(0);

                                                bool isFunction = funcExpContext != null;

                                                string fieldName = propertyNameContext.identifierName().GetText();

                                                ObjectInfo fieldObjInfo = new ObjectInfo() { Type= isFunction? ObjectType.Method:  ObjectType.Field, Name= fieldName, Parent= classObjInfo };
                                                classObjInfo.Children.Add(fieldObjInfo);
                                                this.jsFileInfo.ObjectInfos.Add(fieldObjInfo);
                                            }
                                        }                                       
                                    }
                                }
                            }
                        }
                    }
                    else if (functionContext != null)
                    {
                        string methodName = functionContext.Identifier().GetText();
                        ObjectInfo methodObjInfo = new ObjectInfo() { Type = ObjectType.Method, Name = methodName };
                        this.jsFileInfo.ObjectInfos.Add(methodObjInfo);
                    }
                    else if (importContext != null)
                    {
                        ImportFromBlockContext fromContext = importContext.importFromBlock();
                        ModuleItemsContext[] moduleItems = fromContext.GetRuleContexts<ModuleItemsContext>();
                        foreach (ModuleItemsContext module in moduleItems)
                        {
                            AliasNameContext[] aliasNames = module.aliasName();
                            foreach (AliasNameContext aliasName in aliasNames)
                            {
                                string name = aliasName.identifierName().LastOrDefault().GetText();
                                this.jsFileInfo.Imports.Add(name);
                            }
                        }
                    }
                    else if (exportContext != null)
                    {
                        DeclarationContext[] declarations = exportContext.GetRuleContexts<DeclarationContext>();
                        ExportFromBlockContext[] exportBlocks = exportContext.GetRuleContexts<ExportFromBlockContext>();

                        foreach (DeclarationContext declaration in declarations)
                        {
                            FunctionDeclarationContext funcContext = declaration.functionDeclaration();

                            if (funcContext != null)
                            {
                                string name = funcContext.Identifier().GetText();

                                this.jsFileInfo.Exports.Add(name);
                            }
                        }

                        foreach (ExportFromBlockContext exportBlock in exportBlocks)
                        {
                            ModuleItemsContext[] moduleItems = exportBlock.GetRuleContexts<ModuleItemsContext>();
                            foreach (ModuleItemsContext module in moduleItems)
                            {
                                AliasNameContext[] aliasNames = module.aliasName();
                                foreach (AliasNameContext aliasName in aliasNames)
                                {
                                    IdentifierNameContext[] identifierNames = aliasName.identifierName();
                                    string name = identifierNames.LastOrDefault().Identifier().GetText();
                                    this.jsFileInfo.Exports.Add(name);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
