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
            context.RegisterSyntaxNodeAction(SimpleAssigmentAnalyser, SyntaxKind.SimpleAssignmentExpression);
            context.RegisterSyntaxNodeAction(VariableDeclarationAnalyser, SyntaxKind.VariableDeclaration);
        }

        private void VariableDeclarationAnalyser(SyntaxNodeAnalysisContext context)
        {
            var semanticModel = context.SemanticModel;
            var localDeclaretionStatement = context.Node as LocalDeclarationStatementSyntax;

            if (localDeclaretionStatement == null) return;
        }

        private void SimpleAssigmentAnalyser(SyntaxNodeAnalysisContext context)
        {
            
        }

        //private void Analyser(SyntaxNodeAnalysisContext context)
        //{
        //    var semanticModel = context.SemanticModel;
        //    var statement = context.Node as VariableDeclarationSyntax;

        //    var variableSymbol = semanticModel.GetSymbolInfo(statement).Symbol;
        //    var assignmentExpressions = FindAssigmentExpressions(semanticModel, statement, variableSymbol);

        //    if (assignmentExpressions.Count > 0)
        //    {
        //        var diagnostic = Diagnostic.Create(Rule, statement.GetLocation(), "Assign alignment.", "Align '=' symbols in assign declarations.");
        //        context.ReportDiagnostic(diagnostic);
        //    }
        //}

        //private List<VariableDeclarationSyntax> FindAssigmentExpressions(SemanticModel semanticModel, VariableDeclarationSyntax statement, ISymbol variableSymbol)
        //{
        //    var blockParent = statement.FirstAncestorOrSelf<BlockSyntax>();

        //    if (blockParent == null) return null;

        //    var assignmentExpressions = new List<VariableDeclarationSyntax>();

        //    for (int i = 0; i < blockParent.Statements.Count; i++)
        //    {
        //        var currentStatement = blockParent.Statements[i];
                
        //        if (currentStatement != null && currentStatement.GetText() == statement.GetText())
        //        {
        //            assignmentExpressions.AddRange(CheckNearStatements(blockParent.Statements, i));
        //        }
        //    }




        //    //var indexOfStatement = blockParent.Statements.Equals(statement);

        //    ////Next statement is a LocalDeclaration?
        //    //if (blockParent.Statements[indexOfStatement + 1] is VariableDeclarationSyntax)
        //    //    return null;

        //    //for (int i = indexOfStatement; i >= 0; i--)
        //    //{





        //    //    var blockStatement = blockParent.Statements[i];

        //    //    if (blockParent.Statements.Count - 1 < i && !(blockParent.Statements[i + 1] is VariableDeclarationSyntax))
        //    //        break;

        //    //    var localDeclarationStatement = blockStatement as VariableDeclarationSyntax;
        //    //    if (localDeclarationStatement == null) break;

        //    //    assignmentExpressions.Add(localDeclarationStatement);
        //    //}

        //    return assignmentExpressions;
        //}

        //private List<VariableDeclarationSyntax> CheckNearStatements(SyntaxList<StatementSyntax> statements, int index)
        //{
        //    var localDeclarationStatements = new List<VariableDeclarationSyntax>();

        //    if (!(statements[index] is VariableDeclarationSyntax))
        //    {
        //        return localDeclarationStatements;
        //    }

        //    //if statement is the last statement of code block or next statement isn't a LocalDeclarationStatement
        //    //check if previous one is a LocalDeclarationStatement. If so, this need to be inserted in list
        //    if (statements.Count - 1 == index && statements[index] is VariableDeclarationSyntax)
        //    {
        //        localDeclarationStatements.Add(statements[index] as VariableDeclarationSyntax);
        //        localDeclarationStatements.AddRange(CheckNearStatements(statements, index - 1));
        //    }
        //    else if (!(statements[index + 1] is VariableDeclarationSyntax))
        //    {
        //        localDeclarationStatements.Add(statements[index] as VariableDeclarationSyntax);
        //        localDeclarationStatements.AddRange(CheckNearStatements(statements, index - 1));
        //    }
        //    else if ((statements[index - 1] is VariableDeclarationSyntax))
        //    {
        //        localDeclarationStatements.Add(statements[index] as VariableDeclarationSyntax);
        //        localDeclarationStatements.AddRange(CheckNearStatements(statements, index - 1));
        //    }

        //    return localDeclarationStatements;
        //}
    }
}
