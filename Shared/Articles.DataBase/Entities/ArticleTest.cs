using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Articles.Database.Entities;

//public class ArticleTest : Entity
//{
//    public string FullName { get; set; } = string.Empty;
//    public string Title { get; set; } = string.Empty;
//    public string Content { get; set; } = string.Empty;
//    public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.MinValue;
//    public bool IsApproved { get; set; } = false;
//    public string? RejectionReason { get; set; }
//    public List<Comment> Comments { get; set; } = new();
//}
//public class ArticleTestEntityTypeConfiguration : IEntityTypeConfiguration<ArticleTest>
//{
//    public void Configure(EntityTypeBuilder<ArticleTest> builder)
//    {
//        builder.HasIndex(x => new
//        {
//            x.Title,
//            x.Content
//        }).IsUnique();

//        builder
//            .Property(x => x.FullName)
//            .IsUnicode(false)
//            .IsRequired()
//            .HasMaxLength(50);

//        builder
//            .Property(x => x.Title)
//            .IsUnicode(false)
//            .IsRequired()
//            .HasMaxLength(50);

//        builder
//            .Property(x => x.Content)
//            .IsUnicode(true)
//            .IsRequired()
//            .HasMaxLength(3000);
        
//        builder
//            .Property(x => x.CreatedOn)
//            .IsRequired();

//        builder
//            .Property(x => x.IsApproved)
//            .IsRequired();

//        builder
//            .Property(x => x.RejectionReason)
//            .IsUnicode(false)
//            .HasMaxLength(1000);
//    }
//}