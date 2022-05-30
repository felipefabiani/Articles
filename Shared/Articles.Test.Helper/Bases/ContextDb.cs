using Articles.Database.Context;
using Articles.Database.Entities;
using Articles.Helper.Extensions;
using Articles.Test.Helper.Extensions;
using Articles.Test.Helper.Fixture;
using AutoFixture;
using AutoFixture.Dsl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Immutable;
using Xunit;
using static Articles.Test.Helper.Fixture.ServiceCollectionTestExtension;

namespace Articles.Test.Helper.Bases;

public abstract class ServiceProvider<TClassFixture> : 
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
}
public abstract class ContextDb<TClassFixture> :
    ServiceProvider<TClassFixture>,
    IDisposable,
    IAsyncDisposable
    where TClassFixture : AbstractServiceCollectionFixture
{
    protected ArticleContext _contextWriteOnly;
    protected ArticleReadOnlyContext _contextReadOnly;
    private readonly NexIdService _nextId;
    protected static List<RoleEntity> _roles = new()
    {
        new RoleEntity { Id = 1, Name = "Admin" },
        new RoleEntity { Id = 2, Name = "Author" },
        new RoleEntity { Id = 3, Name = "User" }
    };
    protected static List<ClaimEntity> _claims = new(){
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
    protected static List<UserEntity> _users = new(){
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

    protected static ImmutableList<ArticleEntity> _articles =
        ImmutableList<ArticleEntity>.Empty.AddRange(new List<ArticleEntity>
    {
        new ArticleEntity() {
            Id = 1,
            Title = 20.RandomString(),
            Content = 100.RandomString(),
            IsApproved = false,
            RejectionReason = null,
            // CreatedOn = DateTimeOffset.Now.AddDays(-10),
            AuthorId = 3,
        },
        new ArticleEntity() {
            Id = 2,
            Title = 20.RandomString(),
            Content = 100.RandomString(),
            IsApproved = false,
            RejectionReason = null,
            // CreatedOn = DateTimeOffset.Now.AddDays(-5),
            AuthorId = 3,
        },
        new ArticleEntity() {
            Id = 3,
            Title = 20.RandomString(),
            Content = 100.RandomString(),
            IsApproved = true,
            // CreatedOn = DateTimeOffset.Now.AddDays(-2),
            AuthorId = 3,
        },
        new ArticleEntity() {
            Id = 4,
            Title = 20.RandomString(),
            Content = 100.RandomString(),
            IsApproved = false,
            RejectionReason = "Not good",
            // CreatedOn = DateTimeOffset.Now.AddDays(-1),
            AuthorId = 3,
        },
        new ArticleEntity() {
            Id = 5,
            Title = 20.RandomString(),
            Content = 100.RandomString(),
            IsApproved = true,
            // CreatedOn = DateTimeOffset.Now.AddDays(-1),
            AuthorId = 3,
        },
        new ArticleEntity() {
            Id = 6,
            Title = 20.RandomString(),
            Content = 100.RandomString(),
            IsApproved = false,
            RejectionReason = default,
            // CreatedOn = DateTimeOffset.Now.AddDays(-1),
            AuthorId = 3,
        }
    });

    private static readonly object obj = new object();

    protected ContextDb(TClassFixture spFixture)
        : base(spFixture)
    {
        lock (obj)
        {
            _nextId = _spFixture.ServiceProvider.GetRequiredService<NexIdService>();
            _spFixture.ServiceProvider.GetRequiredService<Testdb>().Reset(GetNextId());
            _contextReadOnly = _spFixture.ServiceProvider.GetRequiredService<IDbContextFactory<ArticleReadOnlyContext>>().CreateDbContext();
            _contextWriteOnly = _spFixture.ServiceProvider.GetRequiredService<IDbContextFactory<ArticleContext>>().CreateDbContext();
            if (!_contextReadOnly.Users.Any())
            {
                InitDb();
                SeedDb();
            }
        }
    }
    protected int GetNextId() => _nextId.GetNextId();
    protected virtual void InitDb()
    {
        lock (obj)
        {
            _contextWriteOnly.Database.EnsureDeleted();
            _contextWriteOnly.Database.EnsureCreated();
            TrackCleanUp();
        }
    }
    protected virtual void SeedDb()
    {
        lock (obj)
        {
            _contextWriteOnly.AddRange(_users);
            _contextWriteOnly.SaveChanges();
            TrackCleanUp();
        }
    }

    private void TrackCleanUp()
    {
        _contextWriteOnly.ChangeTracker.Clear();
        _contextWriteOnly.SaveChanges();
    }

    protected void ArticleSeed()
    {
        if (_contextReadOnly.Articles.Any())
        {
            return;
        }

        var articles = _articles.ToList();
        _contextWriteOnly.AddRange(articles);
        _contextWriteOnly.SaveChanges();

        var dates = new[] { -10, -5, -2, -1, -1, -1 };
        for (int i = 0; i < articles.Count; i++)
        {
            articles[i].CreatedOn = articles[i].CreatedOn.AddDays(dates[i]);
        }

        _contextWriteOnly.UpdateRange(articles);
        _contextWriteOnly.SaveChanges();

        TrackCleanUp();
    }
    protected virtual void Reset()
    {
        _contextWriteOnly.ChangeTracker.Clear();
        _contextWriteOnly.SaveChanges();
        _contextWriteOnly.Database.EnsureDeleted();
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);

        Dispose(disposing: false);
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
        GC.SuppressFinalize(this);
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Reset();

            (_contextWriteOnly as IDisposable)?.Dispose();
            _contextWriteOnly = null!;

            (_contextReadOnly as IDisposable)?.Dispose();
            _contextReadOnly = null!;
        }
    }
    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (_contextReadOnly is not null)
        {
            await _contextReadOnly.DisposeAsync().ConfigureAwait(false);
        }

        if (_contextWriteOnly is IAsyncDisposable disposable)
        {
            await _contextWriteOnly.DisposeAsync().ConfigureAwait(false);
        }

        _contextWriteOnly = null!;
        _contextWriteOnly = null!; ;
    }
}
public abstract class ContextDb<TClassFixture, TEntity> :
    ContextDb<TClassFixture>,
    IClassFixture<TClassFixture>
    where TClassFixture : AbstractServiceCollectionFixture
    where TEntity : Entity<TEntity>, new()
{
    protected readonly IPostprocessComposer<TEntity> _entityBuilder;

    protected ContextDb(TClassFixture spFixture) :
        base(spFixture)

    {
        PreEntityBuilder();

        _entityBuilder = _fixture
            .Build<TEntity>()
            .FromFactory(() => _spFixture.ServiceProvider.GetRequiredService<TEntity>());
    }

    protected virtual void PreEntityBuilder() { }
}
