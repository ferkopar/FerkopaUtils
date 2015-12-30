using System.Text.RegularExpressions;

namespace FerkopaUtils
{
    public static class RegexPatterns
    {
        public static readonly Regex RemoveRepeatedWhitespaceRegex = new Regex(@"(\s|\n|\r){2,}", RegexOptions.Singleline | RegexOptions.IgnoreCase);

      
    }
}
