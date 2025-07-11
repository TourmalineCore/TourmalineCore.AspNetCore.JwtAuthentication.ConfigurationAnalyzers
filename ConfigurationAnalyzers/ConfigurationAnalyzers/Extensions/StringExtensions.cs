namespace ConfigurationAnalyzers.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveWhitespaces(this string sourceString)
        {
            return sourceString.Replace(" ", string.Empty);
        }
    }
}