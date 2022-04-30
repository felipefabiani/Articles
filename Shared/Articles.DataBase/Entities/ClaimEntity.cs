namespace Articles.Database.Entities
{
    public class ClaimEntity : Entity
    {
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public List<UserEntity> Users { get; set; } = null!;
    }

    public class ClaimEntityTypeConfiguration : IEntityTypeConfiguration<ClaimEntity>
    {
        public void Configure(EntityTypeBuilder<ClaimEntity> builder)
        {
            builder.ToTable("Claims");
            builder.HasIndex(x => x.Name).IsUnique();

            builder
                .Property(x => x.Name)
                .IsUnicode(false)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(x => x.Value)
                .IsUnicode(false)
                .IsRequired()
                .HasMaxLength(5);
        }
    }
}