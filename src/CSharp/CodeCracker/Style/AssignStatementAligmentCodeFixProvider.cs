using System;
using System.Linq;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis;
using System.Composition;
using Microsoft.CodeAnalysis.CodeActions;
using System.Threading;

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
			//var declaration		= root.Find 

			context.RegisterFix(CodeAction.Create("Consider to align assign statements", c => AlignStatementsAsync(context.Document, c)), diagnostic);
		}

		private Task<Document> AlignStatementsAsync(/*Document document, CancellationToken c*/)
		{
			throw new NotImplementedException();
		}
	}
}