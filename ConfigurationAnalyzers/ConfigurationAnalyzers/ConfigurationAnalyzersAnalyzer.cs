using ConfigurationAnalyzers.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

namespace ConfigurationAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ConfigurationAnalyzersAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "JwtConf001";
        private static readonly LocalizableString Title = "Using incompatible middlewares of the JwtAuthentication.Core package";
        private static readonly LocalizableString MessageFormat = "It's impossible to use the default login middleware and the cookie login middleware at the same time";
        private static readonly LocalizableString Description = "It can be used either the default login middleware, or the cookie login middleware.";
        private const string Category = "Incompatible middlewares usage";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId,
                Title,
                MessageFormat,
                Category,
                DiagnosticSeverity.Error,
                true,
                Description
            );

        private const string JwtAuthenticationCoreDirectiveName = "TourmalineCore.AspNetCore.JwtAuthentication.Core";
        private const string MethodNameOfDefaultLoginMiddlewareUsage = "UseDefaultLoginMiddleware";
        private const string MethodNameOfCookieLoginMiddlewareUsage = "UseCookieLoginMiddleware";

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxTreeAction(ctx =>
            {
                var root = ctx.Tree.GetRoot();
                var descendantNodes = root.DescendantNodes().ToList();

                var jwtPackageDirectivesIsEnabled = descendantNodes.FirstOrDefault(node => node.IsDirectiveNameEqual(JwtAuthenticationCoreDirectiveName)) != null;

                if (!jwtPackageDirectivesIsEnabled)
                {
                    return;
                }

                var defaultMiddlewareNode = descendantNodes.FirstOrDefault(node => node.IsMethodNameEqual(MethodNameOfDefaultLoginMiddlewareUsage));
                var cookieMiddlewareNode = descendantNodes.FirstOrDefault(node => node.IsMethodNameEqual(MethodNameOfCookieLoginMiddlewareUsage));

                if (defaultMiddlewareNode != null && cookieMiddlewareNode != null)
                {
                    ctx.ReportDiagnostic(Diagnostic.Create(Rule, defaultMiddlewareNode.GetLocation()));
                }
            });
        }
    }
}
