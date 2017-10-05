using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Seed.Common.Extensions
{
    public static class String_Extensions
    {
        public static string DropTrailingSlashes(this string input)
        {
            return input == null
                ? null
                : new Regex("\\/*$").Replace(input, "");
        }

        public static string FirstCharToUpper(this string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("no input");
            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        public static string FirstCharToLower(this string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("no input");
            return input.First().ToString().ToLower() + input.Substring(1);
        }
    }
}