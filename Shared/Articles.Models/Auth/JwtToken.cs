using System;

namespace Articles.Models.Auth
{
    public class JwtToken
    {
        public string Value { get; set; } = string.Empty;
        public DateTimeOffset ExpiryDate { get; set; }
    }
}