using System.Text;

namespace ALMS.API.Helpers
{
    public class RandomStringGenerator(int? seed = null)
    {
        private readonly Random _random = seed.HasValue ? new Random(seed.Value) : new Random();
        private const string DefaultCharset = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public string Generate(int length, string charset = null)
        {
            charset ??= DefaultCharset;

            if (length < 0)
                throw new ArgumentException("Length must be non-negative", nameof(length));

            var stringBuilder = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(charset[_random.Next(charset.Length)]);
            }

            return stringBuilder.ToString();
        }

        public string GenerateAlphanumeric(int length) =>
            Generate(length);

        public string GenerateNumeric(int length) =>
            Generate(length, "0123456789");

        public string GenerateAlphabetic(int length) =>
            Generate(length, "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz");
    }
}
