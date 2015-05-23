using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Imgur.Api.v3.Utilities
{
    internal class StringUtility
    {
        private static readonly Regex RePattern = new Regex(@"(\{+)([^\}]+)(\}+)");

        public static string Format(string pattern, IDictionary<string, string> template)
        {
            if (template == null) throw new ArgumentNullException("template");

            return RePattern.Replace(pattern, match =>
            {
                int lCount = match.Groups[1].Value.Length;
                int rCount = match.Groups[3].Value.Length;

                if ((lCount % 2) != (rCount % 2)) throw new InvalidOperationException("Unbalanced braces");

                string lBrace = lCount == 1 ? string.Empty : new string('{', lCount / 2);
                string rBrace = rCount == 1 ? string.Empty : new string('}', rCount / 2);

                string key = match.Groups[2].Value, value;
                if (lCount % 2 == 0)
                {
                    value = key;
                }
                else
                {
                    if (!template.TryGetValue(key, out value))
                    {
                        throw new ArgumentException("Not found: " + key, "pattern");
                    }
                }
                return lBrace + value + rBrace;
            });
        }

    }
}