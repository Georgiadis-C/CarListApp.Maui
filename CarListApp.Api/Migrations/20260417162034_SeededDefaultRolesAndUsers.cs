using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarListApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeededDefaultRolesAndUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "d1b1a5e8-9c3b-4f1a-8c2e-424gkp3rbv71", "fe1037c6-23eb-477a-80b5-e56fcb320b23", "Administrator", "ADMINISTRATOR" },
                    { "j2h455en-1y8m-6w2c-7r7s-3fk450j963n2", "7a0061a8-c059-464a-bf6c-7248cfd2d41e", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "a1b2c3d4-e5f6-7g8h-9i0j-k1l2m3n4o5p6", 0, "ac689f93-05d8-406c-8f3a-7dc6b0139eca", "admin@localhost.com", true, true, null, "ADMIN@LOCALHOST.COM", "ADMIN@LOCALHOST.COM", "AQAAAAIAAYagAAAAEAw4YboN6xZNjSUwBY8//+UyJ8p4RfEp72GS3GZE3z8yUBZeQoIYRoymMgupCPk8Sg==", null, false, "0ade3fd8-91cd-4581-b795-0f4401b2e39a", false, "admin@localhost.com" },
                    { "p6o5n4m3-l2k1-j0i9-h8g7-f6e5d4c3b2a1", 0, "ff837046-88ff-40a3-9e6b-1e92d7cc5bc3", "user@localhost.com", true, true, null, "USER@LOCALHOST.COM", "USER@LOCALHOST.COM", "AQAAAAIAAYagAAAAEKiIz+Zt1+ag18hpXh7ryfT31EfwNUEWxkwXqCdb033knHvu8bRfS/MJFl6FQsWBpQ==", null, false, "16df81b1-0055-423a-88ee-88210f629839", false, "user@localhost.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "d1b1a5e8-9c3b-4f1a-8c2e-424gkp3rbv71", "a1b2c3d4-e5f6-7g8h-9i0j-k1l2m3n4o5p6" },
                    { "j2h455en-1y8m-6w2c-7r7s-3fk450j963n2", "p6o5n4m3-l2k1-j0i9-h8g7-f6e5d4c3b2a1" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "d1b1a5e8-9c3b-4f1a-8c2e-424gkp3rbv71", "a1b2c3d4-e5f6-7g8h-9i0j-k1l2m3n4o5p6" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "j2h455en-1y8m-6w2c-7r7s-3fk450j963n2", "p6o5n4m3-l2k1-j0i9-h8g7-f6e5d4c3b2a1" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d1b1a5e8-9c3b-4f1a-8c2e-424gkp3rbv71");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "j2h455en-1y8m-6w2c-7r7s-3fk450j963n2");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1b2c3d4-e5f6-7g8h-9i0j-k1l2m3n4o5p6");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "p6o5n4m3-l2k1-j0i9-h8g7-f6e5d4c3b2a1");
        }
    }
}
