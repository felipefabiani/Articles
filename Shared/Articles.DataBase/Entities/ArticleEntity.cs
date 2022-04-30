using Microsoft.Extensions.Logging;

namespace Articles.Database.Entities;

public class ArticleEntity : 
    Entity<ArticleEntity>,
    ITrackableEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsApproved { get; set; } = false;
    public string? RejectionReason { get; set; }
    public List<CommentEntity>? Comments { get; set; }
    public int AuthorId { get; set; }
    public DateTimeOffset CreatedOn { get; set; } = default;
    public DateTimeOffset LastModifyedOn { get; set; }

    public ArticleEntity() : base()
    { }

    public ArticleEntity(IServiceProvider service)
        : base(service)
    { }

    public async Task<ArticleEntity?> Save(CancellationToken cancellationToken = default)
    {
        _logger?.LogDebug("Saving article, id {id}, title {title}", Id, Title);

        if (Id == 0)
        {
            await DbSetWriteOnly.AddAsync(this);
        } 
        else if(DbSetReadOnly.Any(art => art.Id == Id && art.AuthorId == AuthorId))
        {
            DbSetWriteOnly.Update(this);
        }
        else
        {
            return null;
        }

        await SaveAsync(cancellationToken);
        return this;
    }
}
public class ArticleEntityTypeConfiguration : IEntityTypeConfiguration<ArticleEntity>
{
    public void Configure(EntityTypeBuilder<ArticleEntity> builder)
    {
        builder.ToTable("Articles");
        
        builder.HasIndex(x => new
        {
            x.Title,
            x.Content
        }).IsUnique();

        builder
            .Property(x => x.FullName)
            .IsUnicode(false)
            .IsRequired()
            .HasMaxLength(50);

        builder
            .Property(x => x.Title)
            .IsUnicode(false)
            .IsRequired()
            .HasMaxLength(50);

        builder
            .Property(x => x.Content)
            .IsUnicode(true)
            .IsRequired()
            .HasMaxLength(3000);

        builder
            .Property(x => x.CreatedOn)
            .IsRequired();

        builder
            .Property(x => x.IsApproved)
            .IsRequired();

        builder
            .Property(x => x.RejectionReason)
            .IsUnicode(false)
            .HasMaxLength(1000);

        builder
            .HasMany(x => x.Comments)
            .WithOne(x => x.Article)
            .OnDelete(DeleteBehavior.Cascade);
    }
}