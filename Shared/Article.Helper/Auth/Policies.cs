using Microsoft.AspNetCore.Authorization;

namespace Articles.Helper.Auth
{
    public static class Policies
    {
        public static class Author
        {
            public static readonly string AuthorSaveArticle = "AuthorSaveArticle";
            public static AuthorizationPolicy GetAuthorSaveArticle()
            {
                return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireRole("Author")
                    .RequireClaim("Author_Save_Own")
                    .Build();
            }
        }
    }
}
