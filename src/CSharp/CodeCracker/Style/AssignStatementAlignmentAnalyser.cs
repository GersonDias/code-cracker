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

namespace CodeCracker.Style
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AssignStatementAlignmentAnalyser : DiagnosticAnalyzer
    {
		internal const string Title = "Align consecutives assign statements";
		internal const string MessageFormat = "{0}";
		internal const string Category = "Style";
		internal const string Description = "Align consecutives assign statements to improve readability";

		internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(
			DiagnosticId.AssignStatementAligment.ToDiagnosticId(),
			Title,
			MessageFormat,
			Category,
			DiagnosticSeverity.Hidden,
			isEnabledByDefault: true,
			description: Description,
			helpLink: HelpLink.ForDiagnostic(DiagnosticId.AssignStatementAligment)
		);

		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

		public override void Initialize(AnalysisContext context) => context.RegisterSyntaxNodeAction(AnalyseStatmentAlignment, SyntaxKind.VariableDeclaration);

		private void AnalyseStatmentAlignment(SyntaxNodeAnalysisContext context)
		{
			var assignStatement = context.Node as VariableDeclarationSyntax;

			if (assignStatement == null || !assignStatement.ToFullString().Contains("=")) return;

			var parentCodeBlock = assignStatement.Parent.Parent;
			var parentCodeBlockText = parentCodeBlock.GetText();

			for (int i = 0; i < parentCodeBlockText.Lines.Count - 1; i++)
			{
				var currentStatement = parentCodeBlockText.Lines[i].ToString();

				if (currentStatement.Trim() == assignStatement.Parent.ToString())
				{
					var nextStatement = parentCodeBlockText.Lines[i + 1];

					if (nextStatement.ToString().Contains("=") && nextStatement.ToString().IndexOf('=') != currentStatement.IndexOf('='))
					{
						var equals = assignStatement.DescendantTokens().First(x => x.RawKind == (int)SyntaxKind.EqualsToken);
						
						var diagnostic = Diagnostic.Create(Rule, equals.GetLocation(), "Consider to align equals symbols to improve readability");
						context.ReportDiagnostic(diagnostic);
					}
				}
			}

			var currentLine = parentCodeBlock.GetText().Lines.Where(x => x == assignStatement.GetText().Lines.First());
			
			var c = context;
			return;
		}
		
		//private void LocalDeclarationAnalyser(SyntaxNodeAnalysisContext context)
		//{
		//    var semanticModel = context.SemanticModel;

		//    var localDeclaretionStatement = context.Node as LocalDeclarationStatementSyntax;

		//    if (localDeclaretionStatement == null) return;

		//    var parentBlockStatements = localDeclaretionStatement.FirstAncestorOrSelf<BlockSyntax>()?.Statements;

		//    var localDeclarationList = new List<LocalDeclarationStatementSyntax>();
		//    for (int i = 0; i < parentBlockStatements.Value.Count(); i++)
		//    {
		//        var currentStatement = parentBlockStatements.Value[i];

		//        if (currentStatement == localDeclaretionStatement)
		//        {
		//            if (parentBlockStatements.Value.Count - 1 == i)
		//                return;

		//            if (i > 0)
		//            {
		//                var previousStatement = parentBlockStatements.Value[i - 1];
		//                if (previousStatement is LocalDeclarationStatementSyntax)
		//                    break;
		//            }

		//            var nextStatement = parentBlockStatements.Value[i + 1];
		//            if (nextStatement is LocalDeclarationStatementSyntax)
		//            {
		//                localDeclarationList.Add(currentStatement as LocalDeclarationStatementSyntax);
		//                for (int j = i + 1; j < parentBlockStatements.Value.Count(); j++)
		//                {
		//                    if (parentBlockStatements.Value[j] is LocalDeclarationStatementSyntax)
		//                    {
		//                        localDeclarationList.Add(parentBlockStatements.Value[j] as LocalDeclarationStatementSyntax);
		//                    }
		//                }
		//            }

		//            break;
		//        }
		//    }

		//    if (localDeclarationList.Count > 0)
		//    {
		//        if (CheckDeclarationList(localDeclarationList))
		//        {
		//            var diagnostic = Diagnostic.Create(Rule, localDeclaretionStatement.GetLocation(), "Assign alignment.", "Align '=' symbols in assign declarations.");
		//            context.ReportDiagnostic(diagnostic);
		//        }
		//    }
		//}

		//private bool CheckDeclarationList(List<LocalDeclarationStatementSyntax> localDeclarationList)
		//{
		//    var x = "teste";
		//    var x2 = "teste";

		//    if (localDeclarationList.Count > 0)
		//    {
		//        var maxEqualsSymbolSpan = localDeclarationList.Select(x => x.GetText().ToString()).Max(t => t.IndexOf('='));
		//        if (localDeclarationList.Any(x => x.GetText().ToString().IndexOf('=') != maxEqualsSymbolSpan))
		//            return true;
		//    }

		//    return false;
		//}

		//private void VariableDeclarationAnalyser(SyntaxNodeAnalysisContext context)
		//{

		//}

	}
}
