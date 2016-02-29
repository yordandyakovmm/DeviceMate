
namespace DeviceMate.Core.Extensions
{
    public static class StringExtensions
    {
        public static string ToUpperFirstLetter(this string str)
        {
            char[] letters = str.ToCharArray();
            letters[0] = char.ToUpper(letters[0]);
            return new string(letters);
        }
    }
}
