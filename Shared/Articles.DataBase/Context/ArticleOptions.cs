namespace Articles.Database.Context;

public class ArticleOptions
{
    public string SaltId { get; set; } = string.Empty;
    public string JwtSigningKey { get; set; } = string.Empty;
}
