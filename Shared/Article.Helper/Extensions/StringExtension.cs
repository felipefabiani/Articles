using static BCrypt.Net.BCrypt;

namespace Articles.Helper.Extensions
{
    public static class StringExtension
    {
        public static string GetPassword(this string pwd) => EnhancedHashPassword(pwd, 12);

    }
}