using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeFixes;

namespace CodeCracker.Style
{
    public class AssignStatementAligmentCodeFixProvider : CodeFixProvider
    {
        public override Task ComputeFixesAsync(CodeFixContext context)
        {
            return null;
        }

        public override ImmutableArray<string> GetFixableDiagnosticIds()
        {
            return new ImmutableArray<string>();
        }

        public override FixAllProvider GetFixAllProvider()
        {
            return null;
        }
    }
}