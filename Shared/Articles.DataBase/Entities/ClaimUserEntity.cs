//namespace Articles.Database.Entities;

//public class ClaimUserEntity
//{
//    public int UserId { get; set; }
//    public int ClaimId { get; set; }
//    public UserEntity User { get; set; } = new();
//    public ClaimEntity Claim { get; set; } = new();
//}

//public class UserClaimEntityTypeConfiguration : IEntityTypeConfiguration<ClaimUserEntity>
//{
//    public void Configure(EntityTypeBuilder<ClaimUserEntity> builder)
//    {
//        builder.ToTable("ClaimUser");
//        builder.HasIndex(x => new
//        {
//            x.UserId,
//            x.ClaimId,
//        }).IsUnique();
//    }
//}