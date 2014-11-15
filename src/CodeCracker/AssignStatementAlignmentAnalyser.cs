using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;

namespace CodeCracker
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AssignStatementAlignmentAnalyser : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "CodeCracker.AssignStatementAlignmentAnalyser";
        internal const string Title = "Assign Statement Align";
        internal const string MessageFormat = "{0}";
        internal const string Category = "Syntax";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Info, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(Analyser, SyntaxKind.SimpleAssignmentExpression);
            context.RegisterSyntaxNodeAction(Analyser, SyntaxKind.VariableDeclaration);

        }

        private void Analyser(SyntaxNodeAnalysisContext context)
        {
            var semanticModel = context.SemanticModel;
            var statement = context.Node as StatementSyntax;

            var variableSymbol = semanticModel.GetSymbolInfo(statement).Symbol;
            var assignmentExpressions = FindAssigmentExpressions(semanticModel, statement, variableSymbol);

            if (assignmentExpressions.Count > 0)
            {
                var diagnostic = Diagnostic.Create(Rule, statement.GetLocation(), "Assign alignment.", "Align '=' symbols in assign declarations.");
                context.ReportDiagnostic(diagnostic);
            }
        }

        private List<LocalDeclarationStatementSyntax> FindAssigmentExpressions(SemanticModel semanticModel, StatementSyntax statement, ISymbol variableSymbol)
        {
            var blockParent = statement.FirstAncestorOrSelf<BlockSyntax>();

            if (blockParent == null) return null;

            var assignmentExpressions = new List<LocalDeclarationStatementSyntax>();

            var indexOfStatement = blockParent.Statements.IndexOf(statement);

            //Next statement is a LocalDeclaration?
            if (blockParent.Statements[indexOfStatement + 1] is LocalDeclarationStatementSyntax)
                return null;

            for (int i = indexOfStatement; i >= 0; i--)
            {





                var blockStatement = blockParent.Statements[i];

                if (blockParent.Statements.Count - 1 < i && !(blockParent.Statements[i + 1] is LocalDeclarationStatementSyntax))
                    break;

                var localDeclarationStatement = blockStatement as LocalDeclarationStatementSyntax;
                if (localDeclarationStatement == null) break;

                assignmentExpressions.Add(localDeclarationStatement);
            }

            return assignmentExpressions;
        }
    }
}
