namespace Articles.Database.Entities;

public class Role : Entity
{
    public string Name { get; set; } = string.Empty;
    public List<User> Users { get; set; } = null!;

}

public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasIndex(x => x.Name).IsUnique();

        builder
            .Property(x => x.Name)
            .IsUnicode(false)
            .IsRequired()
            .HasMaxLength(50);
    }
}
