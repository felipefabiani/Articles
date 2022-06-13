namespace Articles.Database.View;

public class AuthorView : Entity<AuthorView>
{
    public AuthorView() : base()
    { }

    public AuthorView(IServiceProvider service)
        : base(service)
    { }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public async Task<List<AuthorView>> GetAuthors(CancellationToken cancellationToken) => await DbSetReadOnly
        .ToListAsync(cancellationToken)
        .ConfigureAwait(false);
}

public class AuthorViewTypeConfiguration : IEntityTypeConfiguration<AuthorView>
{
    public void Configure(EntityTypeBuilder<AuthorView> builder)
    {
        builder
            .ToView("Authors")
            .HasKey(t => t.Id);
    }
}