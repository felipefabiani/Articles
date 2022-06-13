//namespace Articles.Database.Entities;

//public class UsersClaimsEntity
//{
//    public int UserId { get; set; }
//    public int ClaimId { get; set; }
//    public UserEntity User { get; set; } = new();
//    public ClaimEntity Claim { get; set; } = new();
//}

//public class UserClaimEntityTypeConfiguration : IEntityTypeConfiguration<UsersClaimsEntity>
//{
//    public void Configure(EntityTypeBuilder<UsersClaimsEntity> builder)
//    {
//        builder.ToTable("UsersClaims");
//        builder.HasIndex(x => new
//        {
//            x.UserId,
//            x.ClaimId,
//        }).IsUnique();
//    }
//}