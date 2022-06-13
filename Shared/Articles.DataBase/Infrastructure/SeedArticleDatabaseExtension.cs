using Articles.Helper.Extensions;

namespace Articles.Database.Infrastructure;

[ExcludeFromCodeCoverage]
public static class SeedArticleDatabaseExtension
{
    public static async ValueTask Seed(this ArticleContext context)
    {
        await context.SeedUsers();
    }
    public static async ValueTask SeedUsers(this ArticleContext context)
    {
        if (await context.Users.AnyAsync())
        {
            return;
        }

        var roles = context.Roles.ToList();
        var claims = context.Claims.ToList();

        var users = new List<UserEntity>
        {
            new UserEntity()
            {
                FirstName = "Full",
                LastName = "Access",
                Email = "full.access@article.ie",
                DateOfBirday = DateTimeOffset.Now.AddYears(-40),
                Password = "123456".GetPassword(),
                Roles = context.Roles.ToList(),
                Claims = claims,
            },
            new UserEntity()
            {
                FirstName = "Admin",
                LastName = "Test",
                Email = "admin.test@article.ie",
                DateOfBirday = DateTimeOffset.Now.AddYears(-40),
                Password = "123456".GetPassword(),
                Roles = context.Roles
                    .Where(x => x.Id == 1 || x.Id == 3)
                    .ToList(),
                Claims = claims.Where(x => x.Id <= 4).ToList()
            },
            new UserEntity()
            {
                FirstName = "Author",
                LastName = "Test",
                Email = "author.test@article.ie",
                DateOfBirday = DateTimeOffset.Now.AddYears(-40),
                Password = "123456".GetPassword(),
                Roles = context.Roles
                    .Where(x => x.Id == 2 || x.Id == 3)
                    .ToList(),
                Claims = claims.Where(x => x.Id >= 5 && x.Id < 9).ToList()
            },
            new UserEntity()
            {
                FirstName = "User",
                LastName = "Test",
                Email = "user.test@article.ie",
                DateOfBirday = DateTimeOffset.Now.AddYears(-40),
                Password = "123456".GetPassword(),
                Roles = context.Roles
                    .Where(x => x.Id == 3)
                    .ToList(),
                Claims = claims.Where(x => x.Id >= 9 && x.Id <= 9).ToList()
            }
        };
        await context.AddRangeAsync(users);
        await context.SaveChangesAsync();
    }

    [ExcludeFromCodeCoverage]
    public static async Task EnsureDropCreateAndSeedAsync(this ArticleContext context)
    {
        await context.Database.EnsureDeletedAsync();
        await EnsureCreateAndSeedAsync(context);
    }

    [ExcludeFromCodeCoverage]
    public static async Task EnsureCreateAndSeedAsync(this ArticleContext context)
    {
        await context.Database.MigrateAsync();
    }
}