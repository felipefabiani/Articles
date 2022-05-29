namespace Articles.Database.Entities;

[ExcludeFromCodeCoverage]
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

        builder
            .HasOne(x => x.Author)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
    }
}