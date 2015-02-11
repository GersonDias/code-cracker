using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using TestHelper;
using Xunit;
using CodeCracker.Style;

namespace CodeCracker.Test
{
    public class AssignSymbolAlignmentTest : CodeFixTest<AssignStatementAlignmentAnalyser, AssignStatementAligmentCodeFixProvider>
    {
		[Fact]
		public async Task WhenThereAreaNoAssignmentStatementNoAnalysisAreCreated()
		{
			const string source = @"
				public void Method()
				{
					Console.WriteLine(""This is a test"");
					Console.ReadLine();
				}";

			await VerifyCSharpHasNoDiagnosticsAsync(source);
		}

		[Fact]
		public async Task WhenThereAreOnlyOneAssignmentNoAnalysisAreCalled()
		{
			const string source = @"
			        public void Method()
			        {
			            var variable = ""x"";
			        }";

			await VerifyCSharpHasNoDiagnosticsAsync(source);
		}

		public async Task WhenThereAreOneAssigmentAndAnotherStatementTypeNoAnalysisAreCalled()
		{
			const string source = @"
			        public void Method()
			        {
			            var variable = ""x"";
						Console.WriteLine(variable);
			        }";
			await VerifyCSharpHasNoDiagnosticsAsync(source);
		}

		[Fact]
		public async Task WhenAssigmentStatementAreAlignedNoAnalysisAreCalled()
		{
			const string source = @"
		        public void Method()
		        {
		            var variable  = ""x"";
		            var variable2 = ""y"";
		        }";

			await VerifyCSharpHasNoDiagnosticsAsync(source);
		}

		[Fact]
		public async Task WhenThereAreManyAssigmentStatementsAnalysisAreCalled()
		{
			const string source = @"
		        public void Method()
		        {
		            var a = ""1"";
		            var ab = ""2"";
					var abc = ""3"";
					var abcd = ""4"";
		        }";

			await VerifyCSharpDiagnosticAsync(source, CreateDiagnostic(4, 19));
		}

		[Fact]
		public async Task WhenAreOnlyVariablesDeclarationWithoutAssigmentNoAnalysisAreCalled()
		{
			const string source = @"
				public void Method()
				{
					int v1, v2, v3;
					string teste;
				}";
			await VerifyCSharpHasNoDiagnosticsAsync(source);
		}

		[Fact]
		public async Task WhenThereAreTwoAssigmentStatementsAnAnalysisAreCalled()
		{
			const string source = @"
				public void Method()
				{
					var variable = ""x"";
					var variable2 = ""y"";
				}";

			await VerifyCSharpDiagnosticAsync(source, CreateDiagnostic(4, 19));
		}

		[Fact]
		public async Task WhenThereAreThreeAssigmentStatementsTwoAnalysisAreCalled()
		{
			const string source = @"
				public void Method()
				{
					var variable = ""x"";
					var variable2 = ""y"";
					var variable32 = ""z"";
				}";

			await VerifyCSharpDiagnosticAsync(source, CreateDiagnostic(4, 19), CreateDiagnostic(5, 20));
		}

		[Fact]
		public async Task WhenAssigmentsAreAlignedNoAnalysisAreCalled()
		{
			const string source = @"
				public void Main()
				{
					var variable1 = ""x"";
					var variable2 = ""y"";
				}";

			await VerifyCSharpHasNoDiagnosticsAsync(source);
		}

		private DiagnosticResult CreateDiagnostic(int line, int column)
		{
			var diagnostic = new DiagnosticResult
			{
				Id = DiagnosticId.AssignStatementAligment.ToDiagnosticId(),
				Message = "Consider to align equals symbols to improve readability",
				Severity = Microsoft.CodeAnalysis.DiagnosticSeverity.Hidden,
				Locations = new [] { new DiagnosticResultLocation("Test0.cs", line, column) }
			};

			return diagnostic;
		}


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
