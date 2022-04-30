namespace Articles.Database.Entities;

public class UserEntity : Entity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTimeOffset DateOfBirday { get; set; } = DateTimeOffset.MinValue;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public List<RoleEntity> Roles { get; set; } = new();
    public List<ClaimEntity> Claims { get; set; } = new();
}

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
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
            .WithMany(x => x.Users)
            .UsingEntity<Dictionary<string, object>>(
                "RoleUser",
                l => l.HasOne<RoleEntity>().WithMany().HasForeignKey("UserId"),
                r => r.HasOne<UserEntity>().WithMany().HasForeignKey("RoleId"),
                j =>
                {
                    j.HasKey("RoleId", "UserId");
                    j.ToTable("RoleUser");
                    j.HasIndex(new[] { "UserId" }, "IX_RoleUser_UserId");
                });

        builder
            .HasMany(x => x.Claims)
            .WithMany(x => x.Users)
            .UsingEntity<Dictionary<string, object>>(
                "ClaimUser",
                l => l.HasOne<ClaimEntity>().WithMany().HasForeignKey("UserId"),
                r => r.HasOne<UserEntity>().WithMany().HasForeignKey("ClaimId"),
                j =>
                {
                    j.HasKey("ClaimId", "UserId");
                    j.ToTable("ClaimUser");
                    j.HasIndex(new[] { "UserId" }, "IX_ClaimUser_UserId");
                });
    }
}