using System;
using System.Collections.Generic;
using System.Text;
using Ghost.Extensions.Extensions;

namespace Ghost.Extensions.Helpers
{
    public static class RandomGenerator
    {
        private static readonly Random rand = new Random();

        private static readonly List<char> letters = new List<char>();
        private static readonly List<char> numbers = new List<char>();
        private static readonly List<char> special = new List<char>();
        private static readonly List<char> space = new List<char>();

        static RandomGenerator()
        {
            for (var i = 65; i <= 90; i++)
            {
                letters.Add((char) i);
            }

            for (var i = 97; i <= 122; i++)
            {
                letters.Add((char)i);
            }

            for (var i = 48; i <= 57; i++)
            {
                numbers.Add((char)i);
            }

            for (var i = 33; i <= 46; i++)
            {
                if (i != 34)
                {
                    special.Add((char)i);                    
                }
            }

            for (var i = 91; i <= 96; i++)
            {
                special.Add((char)i);
            }

            for (var i = 123; i <= 126; i++)
            {
                special.Add((char)i);
            }

            space.Add(' ');
        }

        public static string RandomString(int minLength = 0, int maxLength = int.MaxValue, bool allowLetters = true, bool allowNumbers = true, bool allowSpecialChars = true, bool allowSpaces = true)
        {
            var randLength = rand.Next(minLength, maxLength);
            var sb = new StringBuilder();

            var values = new List<char>();
            if (allowLetters)
            {
                values.AddRange(letters);
            }

            if (allowNumbers)
            {
                values.AddRange(numbers);
            }

            if (allowSpecialChars)
            {
                values.AddRange(special);
            }

            if (allowSpaces)
            {
                values.AddRange(space);
            }

            for (var i = 0; i < randLength; i++)
            {
                sb.Append(values.Random());
            }

            return sb.ToString();
        }

        private static bool RandomBool()
        {
            return new[] {false, true}.Random();
        }

        private static string RandomFromList(IList<string> strings)
        {
            return strings.Random();
        }

        public static DateTime RandomDate(int minYear, int maxYear)
        {
            var year = rand.Next(minYear, maxYear);
            var month = rand.Next(1, 12);
            var day = rand.Next(1, 31);

            return new DateTime(year, month, 1).AddDays(day);
        }

        public static string RandomDateString(int minYear, int maxYear)
        {
            return RandomDate(minYear, maxYear).ToShortDateString();
        }

        public static IEnumerable<string> RandomStrings(int rows, int minLength = 0, int maxLength = int.MaxValue, bool allowLetters = true, bool allowNumbers = true, bool allowSpecialChars = true, bool allowSpaces = true)
        {
            for (var i = 0; i < rows; i++)
            {
                yield return RandomString(minLength, maxLength, allowLetters, allowNumbers, allowSpecialChars, allowSpaces);
            }
        }

        public static IEnumerable<bool> RandomBools(int rows)
        {
            for (var i = 0; i < rows; i++)
            {
                yield return RandomBool();
            }
        }

        public static IEnumerable<string> RandomsFromList(int rows, string[] strings)
        {
            for (var i = 0; i < rows; i++)
            {
                yield return RandomFromList(strings);
            }
        }

        public static IEnumerable<DateTime> RandomDates(int rows, int minYear, int maxYear)
        {
            for (var i = 0; i < rows; i++)
            {
                yield return RandomDate(minYear, maxYear);
            }
        }

        public static IEnumerable<string> RandomDateStrings(int rows, int minYear, int maxYear)
        {
            for (var i = 0; i < rows; i++)
            {
                yield return RandomDateString(minYear, maxYear);
            }
        }
    }
}
