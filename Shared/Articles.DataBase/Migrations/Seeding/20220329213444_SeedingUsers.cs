using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Articles.Database.Migrations.Seeding
{
    public partial class SeedingUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
               table: "Roles",
               columns: new[] { "Id", "Name" },
               values: new object[,] {
                   { 1, "Admin" },
                   { 2, "Author" },
                   { 3, "User" },
               });

            migrationBuilder.InsertData(
                table: "Claims",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,] {
                   { 1, "Article_Moderate", "100" },
                   { 2, "Article_Delete", "101" },
                   { 3, "Article_Get_Pending_List", "102" },
                   { 4, "Article_Update", "103" },
                   { 5, "Author_Update_Profile", "104" },
                   { 6, "Author_Get_Own_List", "200" },
                   { 7, "Author_Save_Own", "201" },
                   { 8, "Author_Update_Own_Profile", "202" },
                   { 9, "User_Reads", "301" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
               table: "Roles",
               keyColumn: "Id",
               keyValue: new object[] { 1, 2, 3 }
            );

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }
            );
        }
    }
}
