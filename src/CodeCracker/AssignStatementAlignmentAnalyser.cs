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
            context.RegisterSyntaxNodeAction(LocalDeclarationAnalyser, SyntaxKind.LocalDeclarationStatement);
            //context.RegisterSyntaxNodeAction(VariableDeclarationAnalyser, SyntaxKind.VariableDeclaration);
        }

        private void LocalDeclarationAnalyser(SyntaxNodeAnalysisContext context)
        {
            var semanticModel = context.SemanticModel;

            var localDeclaretionStatement = context.Node as LocalDeclarationStatementSyntax;

            if (localDeclaretionStatement == null) return;

            var parentBlockStatements = localDeclaretionStatement.FirstAncestorOrSelf<BlockSyntax>()?.Statements;

            var localDeclarationList = new List<LocalDeclarationStatementSyntax>();
            for (int i = 0; i < parentBlockStatements.Value.Count(); i++)
            {
                var currentStatement = parentBlockStatements.Value[i];

                if (currentStatement == localDeclaretionStatement)
                {
                    if (parentBlockStatements.Value.Count - 1 == i)
                        return;

                    if (i > 0)
                    {
                        var previousStatement = parentBlockStatements.Value[i - 1];
                        if (previousStatement is LocalDeclarationStatementSyntax)
                            break;
                    }

                    var nextStatement = parentBlockStatements.Value[i+1];
                    if (nextStatement is LocalDeclarationStatementSyntax)
                    {
                        localDeclarationList.Add(currentStatement as LocalDeclarationStatementSyntax);
                        for(int j = i+1; j < parentBlockStatements.Value.Count(); j++)
                        {
                            if (parentBlockStatements.Value[j] is LocalDeclarationStatementSyntax)
                            {
                                localDeclarationList.Add(parentBlockStatements.Value[j] as LocalDeclarationStatementSyntax);
                            }
                        }
                    }

                    break;
                }
            }

            if (localDeclarationList.Count > 0)
            {
                if (CheckDeclarationList(localDeclarationList))
                {
                    var diagnostic = Diagnostic.Create(Rule, localDeclaretionStatement.GetLocation(), "Assign alignment.", "Align '=' symbols in assign declarations.");
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }

        private bool CheckDeclarationList(List<LocalDeclarationStatementSyntax> localDeclarationList)
        {
            //var x = "teste";
            //var x2 = "teste";

            if (localDeclarationList.Count > 0)
            {
                var maxEqualsSymbolSpan = localDeclarationList.Select(x => x.GetText().ToString()).Max(t => t.IndexOf('='));
                if (localDeclarationList.Any(x => x.GetText().ToString().IndexOf('=') != maxEqualsSymbolSpan))
                    return true;
            }

            return false;
        }

        private void VariableDeclarationAnalyser(SyntaxNodeAnalysisContext context)
        {
            
        }
    }
}
