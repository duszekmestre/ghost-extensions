using System.Linq;
using System.Security.Cryptography;

namespace Ghost.Extensions.Helpers
{
    public static class CodeGeneratorHelper
    {
        public static string GenerateRandomCode(int length, bool onlyNumbers = false)
        {
            char[] alphabet;

            if (onlyNumbers)
            {
                alphabet = Enumerable.Range('0', '9' - '0' + 1).Select(p => (char)p).ToArray();
            }
            else
            {
                alphabet = Enumerable.Range('A', 'X' - 'A' + 1).Concat(Enumerable.Range('0', '9' - '0' + 1)).Select(n => (char)n).ToArray();
            }

            var generator = RandomNumberGenerator.Create();
            var randomBytes = new byte[length];
            generator.GetBytes(randomBytes);
            var randomCode = new string(randomBytes.Select(b => alphabet[b % alphabet.Length]).ToArray());

            return randomCode;
        }
    }
}
