using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMSBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpDateMediaFolder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaFiles_MediaFolders_FolderId",
                table: "MediaFiles");

            migrationBuilder.DropIndex(
                name: "IX_MediaFiles_FolderId",
                table: "MediaFiles");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "MediaFolders",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "FullPath",
                table: "MediaFolders",
                newName: "Path");

            migrationBuilder.RenameColumn(
                name: "FolderId",
                table: "MediaFiles",
                newName: "ModifiedByUserId");

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedByUserId",
                table: "MediaFolders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "PathId",
                table: "MediaFolders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AltText",
                table: "MediaFiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Caption",
                table: "MediaFiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Provider",
                table: "MediaFiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MediaFileFolderLink",
                columns: table => new
                {
                    MediaFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MediaFolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaFileFolderLink", x => new { x.MediaFolderId, x.MediaFileId });
                    table.ForeignKey(
                        name: "FK_MediaFileFolderLink_MediaFiles_MediaFileId",
                        column: x => x.MediaFileId,
                        principalTable: "MediaFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MediaFileFolderLink_MediaFolders_MediaFolderId",
                        column: x => x.MediaFolderId,
                        principalTable: "MediaFolders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MediaFileFolderLink_MediaFileId_MediaFolderId",
                table: "MediaFileFolderLink",
                columns: new[] { "MediaFileId", "MediaFolderId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaFileFolderLink");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId",
                table: "MediaFolders");

            migrationBuilder.DropColumn(
                name: "PathId",
                table: "MediaFolders");

            migrationBuilder.DropColumn(
                name: "AltText",
                table: "MediaFiles");

            migrationBuilder.DropColumn(
                name: "Caption",
                table: "MediaFiles");

            migrationBuilder.DropColumn(
                name: "Provider",
                table: "MediaFiles");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MediaFolders",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Path",
                table: "MediaFolders",
                newName: "FullPath");

            migrationBuilder.RenameColumn(
                name: "ModifiedByUserId",
                table: "MediaFiles",
                newName: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_FolderId",
                table: "MediaFiles",
                column: "FolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaFiles_MediaFolders_FolderId",
                table: "MediaFiles",
                column: "FolderId",
                principalTable: "MediaFolders",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
