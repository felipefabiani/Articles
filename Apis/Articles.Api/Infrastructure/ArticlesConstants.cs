namespace Articles.Api.Infrastructure;

public partial class ArticlesConstants
{
    public static readonly string ArticlesOptions = "ArticleOptions";
    public static readonly string ConnectionString = "ArticlesContext";
    public static readonly string LogFilePath = "log/log-.txt";
    public static readonly int CachingTime = 20 * 60;
}

public partial class ArticlesConstants
{
    public class Environment
    {
        public static readonly string Dev = "Development";
        public static readonly string Uat = "UAT";
        public static readonly string UatAutomatedTest = "UAT-AutomatedTest";
        public static readonly string Production = "Production";
    }
}
public partial class ArticlesConstants
{
    public class Cors
    {
        public static readonly string ArticlesClient = "Article.Client";
        public static readonly string AllowedHostsSection = "AllowedHosts";
    }
    public class Security
    {
        public static readonly string Issuer = "Article.Issuer";
        public static readonly string Audience = "Article.Audience";
    }
}