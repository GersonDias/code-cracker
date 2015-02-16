using System.Threading.Tasks;
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

			await VerifyCSharpDiagnosticAsync(source, 
				CreateDiagnostic(4, 12),
				CreateDiagnostic(5, 13),
				CreateDiagnostic(6, 14));
		}

		[Fact]
		public async Task WhenThereAreMixedStatementsAnalysisAreCreatedCorrectly()
		{
			const string source = @"
				public void Method()
				{
					var a = ""1"";
					Console.WriteLine(a);
					var b = ""3"";

					var bc = ""4"";
					string abc = ""5"";
				}";

			await VerifyCSharpDiagnosticAsync(source,
				CreateDiagnostic(8, 13));
		}

		//[Fact]
		public async Task WhenStatementsAreNotAlignedFixCanDoTheWork()
		{
			const string source = @"
				public void Method()
				{
					var a = ""1"";
					var abc = ""2"";
				}";

			const string fix = @"
				public void Method()
				{
					var a   = ""1"";
					var abc = ""2"";
				}";

			await VerifyCSharpFixAsync(source, fix);
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
				Message = "Align consecutives assign statements could improve readability",
				Severity = Microsoft.CodeAnalysis.DiagnosticSeverity.Hidden,
				Locations = new [] { new DiagnosticResultLocation("Test0.cs", line, column) }
			};

			return diagnostic;
		}
	}
}
