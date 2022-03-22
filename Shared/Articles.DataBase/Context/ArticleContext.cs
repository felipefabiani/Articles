using System.Reflection;

namespace Articles.Database.Context;
public class ArticleContext : DbContext
{
    public ArticleContext(DbContextOptions<ArticleContext> options)
        : base(options)
    {
    }

    // public DbSet<ArticleTest> ArticleTests { get; set; } = null!;
    public DbSet<Article> Articles { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Claim> Claims { get; set; } = null!;
    //public DbSet<UserClaim> UserClaims { get; set; } = null!;
    //public DbSet<UserRole> UserRoles { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
