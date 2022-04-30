namespace Articles.Database.Entities;

public class RoleEntity : Entity
{
    public string Name { get; set; } = string.Empty;
    public List<UserEntity> Users { get; set; } = null!;

}

public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<RoleEntity>
{
    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.ToTable("Roles");
        builder.HasIndex(x => x.Name).IsUnique();

        builder
            .Property(x => x.Name)
            .IsUnicode(false)
            .IsRequired()
            .HasMaxLength(50);
    }
}
