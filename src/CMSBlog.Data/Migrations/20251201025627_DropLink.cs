using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMSBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class DropLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "MediaFileFolderLink");

            migrationBuilder.CreateTable(
                name: "MediaSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActiveProvider = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaSettings", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "MediaSettings",
                columns: new[] { "Id", "ActiveProvider", "UpdatedAt" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), "Local", new DateTime(2025, 12, 1, 2, 56, 27, 1, DateTimeKind.Utc).AddTicks(6852) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaSettings");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "MediaFileFolderLink",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
