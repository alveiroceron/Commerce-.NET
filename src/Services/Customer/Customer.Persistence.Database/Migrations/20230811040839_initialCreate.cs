using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Customer.Persistence.Database.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Customer");

            migrationBuilder.CreateTable(
                name: "Clients",
                schema: "Customer",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ClientId);
                });

            migrationBuilder.InsertData(
                schema: "Customer",
                table: "Clients",
                columns: new[] { "ClientId", "Name" },
                values: new object[,]
                {
                    { 1, "Client 1" },
                    { 2, "Client 2" },
                    { 3, "Client 3" },
                    { 4, "Client 4" },
                    { 5, "Client 5" },
                    { 6, "Client 6" },
                    { 7, "Client 7" },
                    { 8, "Client 8" },
                    { 9, "Client 9" },
                    { 10, "Client 10" },
                    { 11, "Client 11" },
                    { 12, "Client 12" },
                    { 13, "Client 13" },
                    { 14, "Client 14" },
                    { 15, "Client 15" },
                    { 16, "Client 16" },
                    { 17, "Client 17" },
                    { 18, "Client 18" },
                    { 19, "Client 19" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clients",
                schema: "Customer");
        }
    }
}
