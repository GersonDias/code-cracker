using System;
using System.Linq;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis;
using System.Composition;
using Microsoft.CodeAnalysis.CodeActions;
using System.Threading;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace CodeCracker.Style
{
	[ExportCodeFixProvider("CodeCrackerAssignStatementAligmentCodeFixProvider", LanguageNames.CSharp), Shared]
    public class AssignStatementAligmentCodeFixProvider : CodeFixProvider
    {
		public sealed override ImmutableArray<string> GetFixableDiagnosticIds() => ImmutableArray.Create(DiagnosticId.AssignStatementAligment.ToDiagnosticId());
		public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

		public sealed override async Task ComputeFixesAsync(CodeFixContext context)
		{
			var root			= await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
			var diagnostic		= context.Diagnostics.First();
			var diagnosticSpan  = diagnostic.Location.SourceSpan;
			var declaration     = root.DescendantNodes().Where(x => x.RawKind == (int)SyntaxKind.VariableDeclaration);

			context.RegisterFix(CodeAction.Create("Align assign statements", c => AlignStatementsAsync(context.Document, declaration, c)), diagnostic);
		}

		private async Task<Document> AlignStatementsAsync(Document document, IEnumerable<SyntaxNode> statements, CancellationToken cancellationToken)
		{
			var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
			var solution = new StringBuilder();

			//var desc = statements.SelectMany(x => x.DescendantTokens());

			var higherStatement = statements.Max(x => x.ToFullString().IndexOf('='));
			var higherSpan = higherStatement;
			var root = await document.GetSyntaxRootAsync();
			var newDocument = document;
			
			foreach (var statement in statements)
			{
				var equalsToken = statement.DescendantTokens().First(x => x.RawKind == (int)SyntaxKind.EqualsToken);

                if (statement.ToFullString().IndexOf('=') < higherSpan)
				{
					var strStatement = statement.ToFullString();
					var name = strStatement.Substring(0, strStatement.IndexOf('=')).TrimEnd();
					var value = strStatement.Substring(strStatement.IndexOf('='));

					name = name.PadRight(name.Length + (higherSpan - strStatement.IndexOf('=')));
					solution.AppendLine(name + value);
					var text = new TextChange(equalsToken.Span, "".PadRight(higherSpan - statement.ToFullString().IndexOf('=')));

					var spaces = " ".PadRight(higherSpan - statement.ToFullString().IndexOf('=') + 1, ' ');
                    var newRoot = root.ReplaceTrivia(statement.DescendantTrivia().ElementAt(2), SyntaxFactory.ParseTrailingTrivia(spaces));
					newDocument = newDocument.WithSyntaxRoot(newRoot);
				}
				/*else
				{
					solution.AppendLine(statement.ToFullString());
				}*/
			}
			
			return newDocument;
		}
	}
}