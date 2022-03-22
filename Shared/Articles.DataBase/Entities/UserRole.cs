//namespace Articles.Database.Entities;

//public class UserRole : Entity
//{
//    public int UserId { get; set; }
//    public int RoleId { get; set; }

//    public User User { get; set; } = new();
//    public Role Role { get; set; } = new();
//}
//public class UserRoleEntityTypeConfiguration : IEntityTypeConfiguration<UserRole>
//{
//    public void Configure(EntityTypeBuilder<UserRole> builder)
//    {
//        builder.HasIndex(x => new
//        {
//            x.UserId,
//            x.RoleId,
//        }).IsUnique();
//    }
//}