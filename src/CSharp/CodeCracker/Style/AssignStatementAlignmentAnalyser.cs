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

			var parentCodeBlockText = assignStatement.Parent.Parent.GetText();

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
		}
	}
}
