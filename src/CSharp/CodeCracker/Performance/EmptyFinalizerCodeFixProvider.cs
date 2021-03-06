﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CodeCracker.Performance
{
    [ExportCodeFixProvider("CodeCrackerEmptyFinalizerCodeFixProvider", LanguageNames.CSharp), Shared]
    public class EmptyFinalizerCodeFixProvider : CodeFixProvider
    {

        public sealed override ImmutableArray<string> GetFixableDiagnosticIds() =>
            ImmutableArray.Create(DiagnosticId.EmptyFinalizer.ToDiagnosticId());

        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task ComputeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var diagnostic = context.Diagnostics.First();
            var sourceSpan = diagnostic.Location.SourceSpan;
            var finalizer = root.FindToken(sourceSpan.Start).Parent.AncestorsAndSelf().OfType<DestructorDeclarationSyntax>().First();

            context.RegisterFix(
                CodeAction.Create("Remove finalizer", ct => RemoveThrowAsync(context.Document, finalizer, ct)), diagnostic);
        }

        private async Task<Document> RemoveThrowAsync(Document document, DestructorDeclarationSyntax finalizer, CancellationToken ct)
        {
            return document.WithSyntaxRoot((await document.GetSyntaxRootAsync(ct)).RemoveNode(finalizer, SyntaxRemoveOptions.KeepNoTrivia));
        }
    }
}