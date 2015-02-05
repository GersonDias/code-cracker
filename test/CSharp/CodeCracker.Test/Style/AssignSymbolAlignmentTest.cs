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
        //[Fact]
        //public void WhenThereAreNoAssignmentStatementNoAnalysisAreCalled()
        //{
        //    const string source = @"
        //        public void Method() 
        //        {
        //            Console.WriteLine(""This is a test"");
        //            Console.ReadLine();
        //        };";

        //    VerifyCSharpHasNoDiagnostics(source);
        //}

        //[Fact]
        //public void WhenthereAreOnlyOneAssignmentStatementNoAnalysisAreCalled()
        //{
        //    const string source = @"
        //        public void Method()
        //        {
        //            var variable = ""x"";
        //        }";

        //    VerifyCSharpHasNoDiagnostics(source);
        //}

        //[Fact]
        //public void WhenThereAreOneAssigmentAndAnotherStatementTypeNoAnalysisAreCalled()
        //{
        //    const string source = @"
        //        public void Method()
        //        {
        //            var variable = ""x"";
        //            Console.WriteLine(variable);
        //        }";

        //    VerifyCSharpHasNoDiagnostics(source);
        //}

        //[Fact]
        //public void WhenThereAreTwoAssignmentStatementAnAnalysisAreCalled()
        //{
        //    const string source = @"
        //        public void Method()
        //        {
        //            var variable = ""x"";
        //            var variable2 = ""y"";
        //        }";

        //    var expected = new DiagnosticResult
        //    {
        //        Id        = AssignStatementAlignmentAnalyser.DiagnosticId,
        //        Message   = "Assign alignment.",
        //        Severity  = Microsoft.CodeAnalysis.DiagnosticSeverity.Info,
        //        Locations = new[] { new DiagnosticResultLocation("Test0.cs", 4, 21) }
        //    };

        //    VerifyCSharpDiagnostic(source, expected);
        //}

        //[Fact]
        //public void WhenThereAreThreeAssignmentStatementAnAnalysisAreCalled()
        //{
        //    const string source = @"
        //        public void Method()
        //        {
        //            var variable = ""x"";
        //            var variable2 = ""y"";
        //            var variable3  = ""z"";
        //        }";

        //    var expected = new DiagnosticResult
        //    {
        //        Id = AssignStatementAlignmentAnalyser.DiagnosticId,
        //        Message = "Assign alignment.",
        //        Severity = Microsoft.CodeAnalysis.DiagnosticSeverity.Info,
        //        Locations = new[] { new DiagnosticResultLocation("Test0.cs", 4, 21) }
        //    };

        //    VerifyCSharpDiagnostic(source, expected);
        //}

        //[Fact]
        //public void WhenAssigmentStatementAreAlignedNoAnalysisAreCalled()
        //{
        //    const string source = @"
        //        public void Method()
        //        {
        //            var variable  = ""x"";
        //            var variable2 = ""y"";
        //        }";

        //    VerifyCSharpHasNoDiagnostics(source);
        //}

        //protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        //{
        //    return new AssignStatementAlignmentAnalyser();
        //}

        //protected override CodeFixProvider GetCSharpCodeFixProvider()
        //{
        //    return new AssignStatementAligmentCodeFixProvider();
        //}
    }
}
