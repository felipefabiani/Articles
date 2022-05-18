using Microsoft.Extensions.DependencyInjection;

namespace Articles.Helper.Auth
{
    public static class PolicyExtension
    {
        public static IServiceCollection AddArticlesAuthorization(this IServiceCollection services)
        {
            return services.AddAuthorizationCore(op =>
            {
                op.AddPolicy(Policies.Author.AuthorSaveArticle, Policies.Author.GetAuthorSaveArticle());
            });
        }
    }
}
