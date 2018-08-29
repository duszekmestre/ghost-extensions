using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Ghost.Extensions.Helpers
{
    public static class SpecialCharacters
    {
        private static readonly Regex notAsciiRegex = new Regex("[^a-zA-Z -]");

        public static readonly List<char> WithSpecialList = new List<char>(new[]
        {
            'ä', 'ö', 'ü', 'Ä', 'Ö', 'Ü', 'ß', 'ï', 'è', 'ù', 'æ',
            'ą', 'Ą', 'ć', 'Ć', 'ę', 'Ę', 'ł', 'Ł', 'ń', 'Ń', 'ó', 'Ó', 'ś', 'Ś', 'ź', 'Ź', 'ż', 'Ż',
            'Š', 'Ž', 'š', 'ž', 'Ÿ', 'Ŕ', 'Á', 'Â', 'Ă', 'Ä', 'Ĺ', 'Ç', 'Č', 'É', 'Ę', 'Ë', 'Ě', 'Í', 'Î', 'Ď', 'Ń',
            'Ň', 'Ó', 'Ô', 'Ő', 'Ö', 'Ř', 'Ů', 'Ú', 'Ű', 'Ü', 'Ý', 'ŕ', 'á', 'â', 'ă', 'ä', 'ĺ', 'ç', 'č', 'é', 'ę',
            'ë', 'ě', 'í', 'î', 'ď', 'ń', 'ň', 'ó', 'ô', 'ő', 'ö', 'ř', 'ů', 'ú', 'ű', 'ü', 'ý', '˙',
            'Ţ', 'ţ', 'Đ', 'đ', 'ß', 'Œ', 'œ', 'Ć', 'ć', 'ľ'
        });

        public static readonly string[] WithNoSpecial = {
                "ae", "oe", "ue", "Ae", "Oe", "Ue", "ss", "i", "e", "u", "ae",
                "a", "A", "c", "C", "e", "E", "l", "L", "n", "N", "o", "O", "s", "S", "z", "Z", "z", "Z",
                "S", "Z", "s", "z", "Y", "A", "A", "A", "A", "A", "A", "C", "E", "E", "E", "E", "I", "I", "I", "I", "N",
                "O", "O", "O", "O", "O", "O", "U", "U", "U", "U", "Y", "a", "a", "a", "a", "a", "a", "c", "e", "e", "e",
                "e", "i", "i", "i", "i", "n", "o", "o", "o", "o", "o", "o", "u", "u", "u", "u", "y", "y",
                "TH", "th", "DH", "dh", "ss", "OE", "oe", "AE", "ae", "u"
        };

        public static string ReplaceSpecialCharacters(this string input, bool removeOtherNotMappedChars = false)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            var result = input.FixSpecialCharacterasAsASCII();

            if (removeOtherNotMappedChars)
            {
                result = notAsciiRegex.Replace(result, "");
            }

            return result;
        }

        private static string PreProcessSpecialCharacters(this string input)
        {
            return input
                .Replace("ß", "ss")
                .Replace("ß", "ss")
                .Replace("æ", "ae")
                .Replace("˙", "y")
                .Replace("Œ", "OE")
                .Replace("œ", "oe");
        }

        public static string FixSpecialCharacterasAsASCII(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            var preInput = input.PreProcessSpecialCharacters();

            return Encoding.ASCII.GetString(Encoding.GetEncoding(1251).GetBytes(preInput));
        }
    }
}