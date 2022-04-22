using Articles.Helper.Extensions;
using Articles.Database.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Articles.Api.Test.Fixture;

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
        var roles = new List<Role>
        {
            new Role { Id = 1, Name = "Admin" },
            new Role { Id = 2, Name = "Author" },
            new Role { Id = 3, Name = "User" }
        };
        var claims = new List<Claim>
        {
            new Claim { Id = 1, Name = "Article_Moderate", Value= "100" },
            new Claim { Id = 2, Name = "Article_Delete", Value= "101" },
            new Claim { Id = 3, Name = "Article_Get_Pending_List", Value= "102" },
            new Claim { Id = 4, Name = "Article_Update", Value= "103" },
            new Claim { Id = 5, Name = "Author_Update_Profile", Value= "104" },
            new Claim { Id = 6, Name = "Author_Get_Own_List", Value= "200" },
            new Claim { Id = 7, Name = "Author_Save_Own", Value= "201" },
            new Claim { Id = 8, Name = "Author_Update_Own_Profile", Value= "202" },
            new Claim { Id = 9, Name = "user_reads", Value= "301" }
        };
        var users = new List<User>
        {
             new User()
           {
               FirstName = "Full",
               LastName = "Access",
               Email = "full.access@article.ie",
               DateOfBirday = DateTimeOffset.Now.AddYears(-40),
               Password = "123456".GetPassword(),
               Roles = roles,
               Claims = claims,
           },
           new User()
           {
               FirstName = "Admin",
               LastName = "Test",
               Email = "admin.test@article.ie",
               DateOfBirday = DateTimeOffset.Now.AddYears(-40),
               Password = "123456".GetPassword(),
               Roles = roles.Where(x => x.Id == 1).ToList(),
               Claims = claims.Where(x => x.Id <= 4).ToList()
           },
           new User()
           {
               FirstName = "Author",
               LastName = "Test",
               Email = "author.test@article.ie",
               DateOfBirday = DateTimeOffset.Now.AddYears(-40),
               Password = "123456".GetPassword(),
               Roles = roles.Where(x => x.Id == 2).ToList(),
               Claims = claims.Where(x => x.Id >= 5 && x.Id < 9).ToList()
           },
           new User()
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

        _context.Database.EnsureCreated();
        _context.AddRange(roles);
        _context.AddRange(claims);
        _context.AddRange(users);
        _context.SaveChanges();
    }
}

public abstract class AbstractServiceCollectionFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; private set; }
    public AbstractServiceCollectionFixture()
    {
        ServiceProvider = BuildServiceProvider();
    }
    protected abstract ServiceProvider BuildServiceProvider();

    public void Dispose() => ServiceProvider?.Dispose();
}