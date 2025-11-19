using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMSBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTableforMediaLibrary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MediaFolders",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FolderName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    SlugName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ParentFolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaFolders", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MediaFolders_MediaFolders_ParentFolderId",
                        column: x => x.ParentFolderId,
                        principalTable: "MediaFolders",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "MediaTags",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SlugName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    TagName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaTags", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MediaFiles",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    SlugName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FileExtension = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MediaType = table.Column<int>(type: "int", maxLength: 100, nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: true),
                    Height = table.Column<int>(type: "int", nullable: true),
                    Duration = table.Column<double>(type: "float", nullable: true),
                    FolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UploadedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaFiles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MediaFiles_AppUsers_UploadedByUserId",
                        column: x => x.UploadedByUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MediaFiles_MediaFolders_FolderId",
                        column: x => x.FolderId,
                        principalTable: "MediaFolders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "MediaFileTags",
                columns: table => new
                {
                    MediaFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MediaTagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaFileTags", x => new { x.MediaFileId, x.MediaTagId });
                    table.ForeignKey(
                        name: "FK_MediaFileTags_MediaFiles_MediaFileId",
                        column: x => x.MediaFileId,
                        principalTable: "MediaFiles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MediaFileTags_MediaTags_MediaTagId",
                        column: x => x.MediaTagId,
                        principalTable: "MediaTags",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_FolderId",
                table: "MediaFiles",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_ID",
                table: "MediaFiles",
                column: "ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_UploadedByUserId",
                table: "MediaFiles",
                column: "UploadedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFileTags_MediaTagId",
                table: "MediaFileTags",
                column: "MediaTagId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFolders_ParentFolderId",
                table: "MediaFolders",
                column: "ParentFolderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaFileTags");

            migrationBuilder.DropTable(
                name: "MediaFiles");

            migrationBuilder.DropTable(
                name: "MediaTags");

            migrationBuilder.DropTable(
                name: "MediaFolders");
        }
    }
}
