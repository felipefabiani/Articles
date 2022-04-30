using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Articles.Database.migrations
{
    public partial class AddUserManyToManyUsinEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClaimUser_Claims_ClaimsId",
                table: "ClaimUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimUser_Users_UsersId",
                table: "ClaimUser");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleUser_Roles_RolesId",
                table: "RoleUser");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleUser_Users_UsersId",
                table: "RoleUser");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "RoleUser",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "RolesId",
                table: "RoleUser",
                newName: "RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleUser_UsersId",
                table: "RoleUser",
                newName: "IX_RoleUser_UserId");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "ClaimUser",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ClaimsId",
                table: "ClaimUser",
                newName: "ClaimId");

            migrationBuilder.RenameIndex(
                name: "IX_ClaimUser_UsersId",
                table: "ClaimUser",
                newName: "IX_ClaimUser_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimUser_Claims_UserId",
                table: "ClaimUser",
                column: "UserId",
                principalTable: "Claims",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimUser_Users_ClaimId",
                table: "ClaimUser",
                column: "ClaimId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleUser_Roles_UserId",
                table: "RoleUser",
                column: "UserId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleUser_Users_RoleId",
                table: "RoleUser",
                column: "RoleId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClaimUser_Claims_UserId",
                table: "ClaimUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimUser_Users_ClaimId",
                table: "ClaimUser");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleUser_Roles_UserId",
                table: "RoleUser");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleUser_Users_RoleId",
                table: "RoleUser");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "RoleUser",
                newName: "UsersId");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "RoleUser",
                newName: "RolesId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleUser_UserId",
                table: "RoleUser",
                newName: "IX_RoleUser_UsersId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ClaimUser",
                newName: "UsersId");

            migrationBuilder.RenameColumn(
                name: "ClaimId",
                table: "ClaimUser",
                newName: "ClaimsId");

            migrationBuilder.RenameIndex(
                name: "IX_ClaimUser_UserId",
                table: "ClaimUser",
                newName: "IX_ClaimUser_UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimUser_Claims_ClaimsId",
                table: "ClaimUser",
                column: "ClaimsId",
                principalTable: "Claims",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimUser_Users_UsersId",
                table: "ClaimUser",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleUser_Roles_RolesId",
                table: "RoleUser",
                column: "RolesId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleUser_Users_UsersId",
                table: "RoleUser",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
