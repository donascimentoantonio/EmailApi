
using System.Text.RegularExpressions;

namespace EmailManagement.Infrastructure.Persistence
{
    public static class Utils
    {
        public static string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }

            var startUnderscores = Regex.Match(input, @"^_+");
            return
                startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }
    }
}
