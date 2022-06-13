using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Articles.Database.Migrations
{
    public partial class AuthorsView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW Authors 
                AS 
                SELECT UserId AS Id
                     , u.FirstName
	                 , u.LastName
                  FROM UsersRoles ur
                 INNER JOIN Users u
                         ON u.Id = ur.UserId
                 INNER JOIN Roles r 
                         ON r.Id = ur.RoleId
                 WHERE 'Author' = r.[Name]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW Authors");
        }
    }
}
