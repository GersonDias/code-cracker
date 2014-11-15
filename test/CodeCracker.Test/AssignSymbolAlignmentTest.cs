using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using TestHelper;
using Xunit;

namespace CodeCracker.Test
{
    public class AssignSymbolAlignmentTest : CodeFixVerifier
    {
        [Fact]
        public void WhenThereAreOnlyOneAssignStatementNoAnalysisAreCalled()
        {
            const string source = @"var st = ""this is a string!"";";

            VerifyCSharpHasNoDiagnostics(source);
        }

        [Fact]
        public void WhenNextLineIsBlanckNoAnalysisAreCalled()
        {
            string source =@"
public void method()
{
    var st = ""this is a string!"";

    Console.WriteLine(st);
}";

            VerifyCSharpHasNoDiagnostics(source);
        }

        [Fact]
        public void WhenNextStatementIsntAssignStatetemeNoAnalysisAreCalled()
        {
            string source = @"
public void method()
{
    var st = ""this is a string!"";
    Console.WriteLine(st.Length);
}";
            VerifyCSharpHasNoDiagnostics(source);
        }

        [Fact]
        public void WhenNextStatementIsAAssignStatementAndAssignSignIsntAllignAnalysisAreCalled()
        {
            string source =
@"public void method()
{
    var st = ""this is a string!"";
    var st2 = ""this is another string!"";
}";

            var expected = new DiagnosticResult
            {
                Id = AssignStatementAlignmentAnalyser.DiagnosticId,
                Message = "Align assign symbol",
                Severity = Microsoft.CodeAnalysis.DiagnosticSeverity.Info,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 2, 8) }
            };

            VerifyCSharpDiagnostic(source, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new AssignStatementAlignmentAnalyser();
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new AssignStatementAligmentCodeFixProvider();
        }
    }
}
