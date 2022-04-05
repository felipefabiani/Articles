namespace Articles.Database.Entities;

public class User : Entity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTimeOffset DateOfBirday { get; set; } = DateTimeOffset.MinValue;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public List<Role> Roles { get; set; } = new();
    public List<Claim> Claims { get; set; } = new();
}

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(x => new
        {
            x.Email,
            x.Password
        }).IsUnique();

        builder
            .Property(x => x.FirstName)
            .IsUnicode(false)
            .IsRequired()
            .HasMaxLength(50);

        builder
            .Property(x => x.LastName)
            .IsUnicode(false)
            .IsRequired()
            .HasMaxLength(50);

        builder
            .Property(x => x.DateOfBirday)
            .IsRequired();

        builder
            .Property(x => x.Email)
            .IsUnicode(false)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .HasIndex(x => x.Email)
            .IsUnique();

        builder
            .Property(x => x.Password)
            .IsUnicode(false)
            .IsRequired()
            .HasMaxLength(60);

        builder
            .HasMany(x => x.Roles)
            .WithMany(x => x.Users);

        builder
            .HasMany(x => x.Claims)
            .WithMany(x => x.Users);
    }
}