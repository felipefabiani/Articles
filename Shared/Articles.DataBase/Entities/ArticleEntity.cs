using Microsoft.Extensions.Logging;

namespace Articles.Database.Entities;

public class ArticleEntity : Entity<ArticleEntity>
{
    public string FullName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.MinValue;
    public bool IsApproved { get; set; } = false;
    public string? RejectionReason { get; set; }
    public List<Comment> Comments { get; set; } = new();
    public int AuthorId { get; set; }

    public ArticleEntity() : base()
    { }

    public ArticleEntity(IServiceProvider service)
        : base(service)
    { }

    public async Task<ArticleEntity?> SaveArticle(CancellationToken cancellationToken = default)
    {
        _logger?.LogDebug("Saving article, id {id}, title {title}", Id, Title);

        if (Id != 0 &&
          !DbSetReadOnly.Any(art => art.Id == Id && art.AuthorId == AuthorId)) 
        {
            return null;
        }

        var art = DbSetWriteOnly.Update(this);
        await SaveAsync(cancellationToken);
        return art.Entity;
    }
}
public class ArticleEntityTypeConfiguration : IEntityTypeConfiguration<ArticleEntity>
{
    public void Configure(EntityTypeBuilder<ArticleEntity> builder)
    {
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
    }
}