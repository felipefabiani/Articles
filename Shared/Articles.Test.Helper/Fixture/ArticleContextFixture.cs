using Articles.Database.Context;
using Articles.Database.Entities;
using Articles.Helper.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Articles.Test.Helper.Fixture;

public abstract class DbAbstractServiceCollectionFixture<TDb> :
    AbstractServiceCollectionFixture
    where TDb : ArticleAbstractContext
{
    public DbAbstractServiceCollectionFixture()
    {
        _context = GetDbContext();
        InitDb();
    }
    protected abstract TDb GetDbContext();
    protected readonly TDb _context = null!;

    protected virtual void InitDb()
    {
        var roles = new List<RoleEntity>
        {
            new RoleEntity { Id = 1, Name = "Admin" },
            new RoleEntity { Id = 2, Name = "Author" },
            new RoleEntity { Id = 3, Name = "User" }
        };
        var claims = new List<ClaimEntity>
        {
            new ClaimEntity { Id = 1, Name = "Article_Moderate", Value= "100" },
            new ClaimEntity { Id = 2, Name = "Article_Delete", Value= "101" },
            new ClaimEntity { Id = 3, Name = "Article_Get_Pending_List", Value= "102" },
            new ClaimEntity { Id = 4, Name = "Article_Update", Value= "103" },
            new ClaimEntity { Id = 5, Name = "Author_Update_Profile", Value= "104" },
            new ClaimEntity { Id = 6, Name = "Author_Get_Own_List", Value= "200" },
            new ClaimEntity { Id = 7, Name = "Author_Save_Own", Value= "201" },
            new ClaimEntity { Id = 8, Name = "Author_Update_Own_Profile", Value= "202" },
            new ClaimEntity { Id = 9, Name = "user_reads", Value= "301" }
        };
        var users = new List<UserEntity>
        {
             new UserEntity()
           {
               FirstName = "Full",
               LastName = "Access",
               Email = "full.access@article.ie",
               DateOfBirday = DateTimeOffset.Now.AddYears(-40),
               Password = "123456".GetPassword(),
               Roles = roles,
               Claims = claims,
           },
           new UserEntity()
           {
               FirstName = "Admin",
               LastName = "Test",
               Email = "admin.test@article.ie",
               DateOfBirday = DateTimeOffset.Now.AddYears(-40),
               Password = "123456".GetPassword(),
               Roles = roles.Where(x => x.Id == 1).ToList(),
               Claims = claims.Where(x => x.Id <= 4).ToList()
           },
           new UserEntity()
           {
               FirstName = "Author",
               LastName = "Test",
               Email = "author.test@article.ie",
               DateOfBirday = DateTimeOffset.Now.AddYears(-40),
               Password = "123456".GetPassword(),
               Roles = roles.Where(x => x.Id == 2).ToList(),
               Claims = claims.Where(x => x.Id >= 5 && x.Id < 9).ToList()
           },
           new UserEntity()
           {
               FirstName = "User",
               LastName = "Test",
               Email = "user.test@article.ie",
               DateOfBirday = DateTimeOffset.Now.AddYears(-40),
               Password = "123456".GetPassword(),
               Roles = roles.Where(x => x.Id == 3).ToList(),
               Claims = claims.Where(x => x.Id >= 9 && x.Id <= 9).ToList()
           }
        };

        // to seed you must use Writable dbContext;
        var content = ServiceProvider.GetRequiredService<ArticleContext>();

        content.Database.EnsureCreated();
        content.AddRange(roles);
        content.AddRange(claims);
        content.AddRange(users);
        content.SaveChanges();
    }
}

public abstract class AbstractServiceCollectionFixture : IDisposable
{
    public IServiceProvider ServiceProvider { get; private set; }
    public AbstractServiceCollectionFixture()
    {
        ServiceProvider = BuildServiceProvider();
    }
    protected abstract IServiceProvider BuildServiceProvider();

    public void Dispose() => (ServiceProvider as ServiceProvider)?.Dispose();
}