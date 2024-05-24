using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wave.Commerce.Persistence.Migrations
{
    public partial class InitialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false),
                    StockQuantity = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name", "Value", "StockQuantity", "CreatedDate", "UpdatedDate" },
                values: new object[,]
                {
                    { Guid.Parse("076d553e-4bf8-4367-931a-e7742501e8b6"), "Apple Iphone 13", 799.99m, 50, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow },
                    { Guid.Parse("3c253a59-f32c-4ff1-9ab7-af245e50e291"), "Samsung Galaxy S21", 699.99m,  70, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow },
                    { Guid.Parse("8270d170-d487-4a1b-8f0c-12eee8ca4a4a"), "Dell XPS 13 Laptop", 999.80m,  150, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow },
                    { Guid.Parse("02664afa-6ab9-4ef7-9619-b3644f9c02e4"), "Amazon Echo Dot (4th Gen)", 60.50m,  350, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow },
                    { Guid.Parse("7bec3fbb-94cb-4d09-976b-d0831b3b9ad8"), "Galaxy Watch pro 5", 1260.50m,  350, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
