namespace Articles.Database.Entities;

public class CommentEntity : Entity, ITrackableEntity
{
    public string NickName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.MinValue;
    public DateTimeOffset LastModifyedOn { get; set; } = DateTimeOffset.MinValue;
    public ArticleEntity Article { get; set; } = null!;
}

public class CommentEntityTypeConfiguration : IEntityTypeConfiguration<CommentEntity>
{
    public void Configure(EntityTypeBuilder<CommentEntity> builder)
    {
        builder.ToTable("Comments");
        builder
            .Property(x => x.NickName)
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
            .Property(x => x.LastModifyedOn)
            .IsRequired();

        builder
            .HasOne(x => x.Article)
            .WithMany(x => x.Comments)
            .IsRequired();
    }
}
