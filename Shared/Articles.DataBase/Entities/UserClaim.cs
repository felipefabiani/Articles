namespace Articles.Database.Entities;

//public class UserClaim : Entity
//{
//    public int UserId { get; set; }
//    public int ClaimId { get; set; }
//    public User User { get; set; } = new();
//    public Claim Role { get; set; } = new();
//}

//public class UserClaimEntityTypeConfiguration : IEntityTypeConfiguration<UserClaim>
//{
//    public void Configure(EntityTypeBuilder<UserClaim> builder)
//    {
//        builder.HasIndex(x => new
//        {
//            x.UserId,
//            x.ClaimId,
//        }).IsUnique();
//    }
//}