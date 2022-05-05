////    For any command bellow do:
////        Select Articles.api as Statup project
////        Package Manager Console select Aricles.Database
////    
////    Add-Migration: 
////        EntityFrameworkCore\Add-Migration {migration-name} -Context ArticleContext -OutputDir Migrations
////    
////    Remove-Migration:
////        EntityFrameworkCore\Remove-Migration -Context ArticleContext -OutputDir Migrations
////    
////    Update-DAtabase: 
////        $env: ASPNETCORE_ENVIRONMENT = '{env}'
////        EntityFrameworkCore\Update-Database -Context ArticleContext
////    
////    Rollback Migration:
////        EntityFrameworkCore\Update-Database -migration { migration} -Context ArticleContext
////
////    Get Script:
////        EntityFrameworkCore\Script-Migration -Context ArticleContext
////    or  EntityFrameworkCore\Script-Migration {*from migration} -Context ArticleContext
////    or  EntityFrameworkCore\Script-Migration {*from migration} {to migration} -Context ArticleContext
////    * from not include the script for the specific migration

using System.Reflection;

namespace Articles.Database.Context;

public abstract class ArticleAbstractContext : DbContext
{
    protected ArticleAbstractContext(DbContextOptions contextOptions)
        : base(contextOptions)
    {
    }

    public virtual DbSet<ArticleEntity> Articles { get; set; } = null!;
    public virtual DbSet<CommentEntity> Comments { get; set; } = null!;
    public virtual DbSet<UserEntity> Users { get; set; } = null!;
    public virtual DbSet<RoleEntity> Roles { get; set; } = null!;
    public virtual DbSet<ClaimEntity> Claims { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
