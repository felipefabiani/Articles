﻿using Articles.Database.Context;
using Articles.Database.Entities;
using Articles.Helper.Extensions;
using Articles.Test.Helper.Fixture;
using AutoFixture;
using AutoFixture.Dsl;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Articles.Test.Helper.Bases;

public abstract class ServiceProvider<TClassFixture> :
    IDisposable,
    IClassFixture<TClassFixture>
    where TClassFixture : AbstractServiceCollectionFixture
{
    protected readonly IFixture _fixture;
    protected readonly TClassFixture _spFixture;

    protected ServiceProvider(TClassFixture spFixture)
    {
        _fixture = new AutoFixture.Fixture();
        _spFixture = spFixture;
    }
    public virtual void Dispose() { }
}
public abstract class ContextDb<TClassFixture> :
    ServiceProvider<TClassFixture>
    where TClassFixture : AbstractServiceCollectionFixture
{
    protected readonly ArticleContext _contextWriteOnly;
    protected readonly ArticleReadOnlyContext _contextReadOnly;
    protected static List<RoleEntity>? _roles;
    protected static List<ClaimEntity>? _claims;
    protected static List<UserEntity>? _users;

    protected ContextDb(TClassFixture spFixture)
        : base(spFixture)
    {
        lock (this)
        {
            _contextWriteOnly = _spFixture.ServiceProvider.GetRequiredService<ArticleContext>();
            _contextReadOnly = _spFixture.ServiceProvider.GetRequiredService<ArticleReadOnlyContext>();
            //_contextWriteOnly = _spFixture.ServiceProvider.GetRequiredService<IDbContextFactory<ArticleContext>>().CreateDbContext();
            //_contextReadOnly = _spFixture.ServiceProvider.GetRequiredService<IDbContextFactory<ArticleReadOnlyContext>>().CreateDbContext();

            InitDb();
            SeedDb();
        }
    }
    public override void Dispose()
    {
        lock (this)
        {
            Reset();
            //_contextReadOnly.Dispose();
            //_contextWriteOnly.Dispose();
        }
    }

    protected virtual void InitDb()
    {
        lock (this)
        {
            _contextWriteOnly.Database.EnsureDeleted();
            _contextWriteOnly.Database.EnsureCreated();
            _contextWriteOnly.ChangeTracker.Clear();
            _contextWriteOnly.SaveChanges();
        }
    }
    protected virtual void SeedDb()
    {
        lock (this)
        {
            _roles = new List<RoleEntity>
        {
            new RoleEntity { Id = 1, Name = "Admin" },
            new RoleEntity { Id = 2, Name = "Author" },
            new RoleEntity { Id = 3, Name = "User" }
        };
            _claims = new List<ClaimEntity>
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
            _users = new List<UserEntity>
        {
            new UserEntity()
           {
               FirstName = "Full",
               LastName = "Access",
               Email = "full.access@article.ie",
               DateOfBirday = DateTimeOffset.Now.AddYears(-40),
               Password = "123456".GetPassword(),
               Roles = _roles,
               Claims = _claims,
           },
           new UserEntity()
           {
               FirstName = "Admin",
               LastName = "Test",
               Email = "admin.test@article.ie",
               DateOfBirday = DateTimeOffset.Now.AddYears(-40),
               Password = "123456".GetPassword(),
               Roles = _roles.Where(x => x.Id == 1).ToList(),
               Claims = _claims.Where(x => x.Id <= 4).ToList()
           },
           new UserEntity()
           {
               FirstName = "Author",
               LastName = "Test",
               Email = "author.test@article.ie",
               DateOfBirday = DateTimeOffset.Now.AddYears(-40),
               Password = "123456".GetPassword(),
               Roles = _roles.Where(x => x.Id == 2).ToList(),
               Claims = _claims.Where(x => x.Id >= 5 && x.Id < 9).ToList()
           },
           new UserEntity()
           {
               FirstName = "User",
               LastName = "Test",
               Email = "user.test@article.ie",
               DateOfBirday = DateTimeOffset.Now.AddYears(-40),
               Password = "123456".GetPassword(),
               Roles = _roles.Where(x => x.Id == 3).ToList(),
               Claims = _claims.Where(x => x.Id >= 9 && x.Id <= 9).ToList()
           }
        };

            _contextWriteOnly.AddRange(_users);
            _contextWriteOnly.SaveChanges();

            _contextWriteOnly.ChangeTracker.Clear();
            _contextWriteOnly.SaveChanges();
        }
    }
    protected virtual void Reset()
    {
        _contextWriteOnly.ChangeTracker.Clear();
        _contextWriteOnly.SaveChanges();
        _contextWriteOnly.Database.EnsureDeleted();
    }
}
public abstract class ContextDb<TClassFixture, TEntity> :
    ContextDb<TClassFixture>,
    IClassFixture<TClassFixture>
    where TClassFixture : AbstractServiceCollectionFixture
    where TEntity : Entity<TEntity>, new()
{
    protected readonly IPostprocessComposer<TEntity> _entityBuilder;

    protected ContextDb(
        TClassFixture spFixture,
        TEntity entityBuilder) :
        base(spFixture)

    {
        PreEntityBuilder();

        _entityBuilder = _fixture
            .Build<TEntity>()
            .FromFactory(() => entityBuilder);
    }

    protected virtual void PreEntityBuilder() { }
}