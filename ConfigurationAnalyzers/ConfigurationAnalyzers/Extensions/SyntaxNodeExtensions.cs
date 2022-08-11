using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace ConfigurationAnalyzers.Extensions
{
    public static class SyntaxNodeExtensions
    {
        public static bool IsMethodNameEqual(this SyntaxNode node, string methodName)
        {
            return node.IsKind(SyntaxKind.SimpleMemberAccessExpression) && 
                   node
                       .ToString()
                       .RemoveWhitespaces()
                       .Contains(methodName);
        }

        public static bool IsDirectiveNameEqual(this SyntaxNode node, string directiveName)
        {
            return node.IsKind(SyntaxKind.UsingDirective) &&
                   node
                       .ToString()
                       .RemoveWhitespaces()
                       .Contains(directiveName);
        }
    }
}