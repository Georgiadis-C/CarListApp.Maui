using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarListApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Make = table.Column<string>(type: "TEXT", nullable: false),
                    Model = table.Column<string>(type: "TEXT", nullable: false),
                    Vin = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "Make", "Model", "Vin" },
                values: new object[,]
                {
                    { 1, "Toyota", "Corolla", "JT4BG22K6V000001" },
                    { 2, "Honda", "Civic", "2HGES16555H000002" },
                    { 3, "Ford", "Mustang", "1FAFP404X1F000003" },
                    { 4, "Chevrolet", "Impala", "2G1WF52E659000004" },
                    { 5, "Nissan", "Altima", "1N4AL11D75C000005" },
                    { 6, "BMW", "320", "WBA3A5C50DF000006" },
                    { 7, "Audi", "A4", "WAUDF78E37A000007" },
                    { 8, "Mercedes-Benz", "C-Class", "WDBRF40JX3F000008" },
                    { 9, "Volkswagen", "Jetta", "3VW2K7AJ5DM000009" },
                    { 10, "Subaru", "Impreza", "JF1GPAL69DH000010" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cars");
        }
    }
}
