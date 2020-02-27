using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CodeParser;
using System.IO;
using System.Linq;
using static CodeParser.PythonParser;

namespace SpeechCodingHandler
{
    public class PythonFileParser : LanguageFileParser
    {
        private PythonFileInfo pythonFileInfo;

        public override string FileExtension => ".py";

        public PythonFileParser() : base() { }

        public PythonFileParser(string filePath):base(filePath)
        {
        }

        public override LanguageFileInfo Parse()
        {
            this.pythonFileInfo = new PythonFileInfo() { FileInfo = new FileInfo(this.FilePath) };

            Lexer lexer = new PythonLexer(CharStreams.fromPath(this.FilePath));

            CommonTokenStream tokens = new CommonTokenStream(lexer);

            PythonParser parser = new PythonParser(tokens);

            ParserRuleContext context = parser.file_input();

            var children = context.children;

            foreach (IParseTree child in children)
            {
                this.ParseNode(child);
            }

            return pythonFileInfo;
        }

        private void ParseNode(IParseTree node)
        {
            if (node is StmtContext)
            {
                StmtContext stmtContext = node as StmtContext;
                Compound_stmtContext compound_Stmt = stmtContext.compound_stmt();

                if (compound_Stmt != null)
                {
                    ClassdefContext[] classdefs = compound_Stmt.GetRuleContexts<ClassdefContext>();

                    foreach (ClassdefContext classdef in classdefs)
                    {
                        this.ParseClassContext(classdef);                        
                    }
                }
            }
        }

        private void ParseClassContext(ClassdefContext node)
        {
            string className = node.name().GetText();

            ObjectInfo classObjInfo = new ObjectInfo() { Type = ObjectType.Class, Name = className };
            this.pythonFileInfo.ObjectInfos.Add(classObjInfo);

            var suits = node.GetRuleContexts<SuiteContext>();

            foreach (SuiteContext suit in suits)
            {
                var stmts = suit.stmt();

                foreach (StmtContext stmt in stmts)
                {
                    Simple_stmtContext simple_Stmt = stmt.simple_stmt();
                    Compound_stmtContext compound_Stmt = stmt.compound_stmt();

                    if (compound_Stmt != null)
                    {
                        var funcs = compound_Stmt.GetRuleContexts<FuncdefContext>();

                        foreach (FuncdefContext func in funcs)
                        {
                            string funcName = func.name().GetText();

                            ObjectInfo funcObjInfo = new ObjectInfo() { Type = ObjectType.Method, Name = funcName, Parent = classObjInfo };
                            classObjInfo.Children.Add(funcObjInfo);
                        }
                    }
                    else if (simple_Stmt != null)
                    {
                        var exprs = simple_Stmt.GetRuleContexts<Expr_stmtContext>();

                        foreach (var expr in exprs)
                        {
                            Testlist_star_exprContext[] ss = expr.GetRuleContexts<Testlist_star_exprContext>();

                            string fieldName = ss.FirstOrDefault()?.GetText();
                            if (!string.IsNullOrEmpty(fieldName))
                            {
                                ObjectInfo fieldObjInfo = new ObjectInfo() { Type = ObjectType.Field, Name = fieldName, Parent = classObjInfo };
                                classObjInfo.Children.Add(fieldObjInfo);
                            }
                        }
                    }
                }
            }

            this.pythonFileInfo.ObjectInfos.AddRange(classObjInfo.Children);
        }
    }
}
