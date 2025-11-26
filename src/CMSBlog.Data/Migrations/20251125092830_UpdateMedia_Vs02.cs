using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMSBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMedia_Vs02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaFiles_AspNetUsers_UploadedByUserId",
                table: "MediaFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaFolders_MediaFolders_ParentFolderId",
                table: "MediaFolders");

            migrationBuilder.DropIndex(
                name: "IX_MediaFiles_UploadedByUserId",
                table: "MediaFiles");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "MediaFiles",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_MediaFiles_ID",
                table: "MediaFiles",
                newName: "IX_MediaFiles_Id");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MediaTags",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "MediaTags",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateModified",
                table: "MediaFolders",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "MediaFolders",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "FullPath",
                table: "MediaFolders",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "MediaType",
                table: "MediaFiles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FileExtension",
                table: "MediaFiles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateModified",
                table: "MediaFiles",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "MediaFiles",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "MediaFiles",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "MimeType",
                table: "MediaFiles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_MediaTags_SlugName",
                table: "MediaTags",
                column: "SlugName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediaFileTags_MediaFileId_MediaTagId",
                table: "MediaFileTags",
                columns: new[] { "MediaFileId", "MediaTagId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_FilePath",
                table: "MediaFiles",
                column: "FilePath");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_MediaType",
                table: "MediaFiles",
                column: "MediaType");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_SlugName",
                table: "MediaFiles",
                column: "SlugName");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaFolders_MediaFolders_ParentFolderId",
                table: "MediaFolders",
                column: "ParentFolderId",
                principalTable: "MediaFolders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaFolders_MediaFolders_ParentFolderId",
                table: "MediaFolders");

            migrationBuilder.DropIndex(
                name: "IX_MediaTags_SlugName",
                table: "MediaTags");

            migrationBuilder.DropIndex(
                name: "IX_MediaFileTags_MediaFileId_MediaTagId",
                table: "MediaFileTags");

            migrationBuilder.DropIndex(
                name: "IX_MediaFiles_FilePath",
                table: "MediaFiles");

            migrationBuilder.DropIndex(
                name: "IX_MediaFiles_MediaType",
                table: "MediaFiles");

            migrationBuilder.DropIndex(
                name: "IX_MediaFiles_SlugName",
                table: "MediaFiles");

            migrationBuilder.DropColumn(
                name: "FullPath",
                table: "MediaFolders");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "MediaFiles");

            migrationBuilder.DropColumn(
                name: "MimeType",
                table: "MediaFiles");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MediaFiles",
                newName: "ID");

            migrationBuilder.RenameIndex(
                name: "IX_MediaFiles_Id",
                table: "MediaFiles",
                newName: "IX_MediaFiles_ID");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MediaTags",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "MediaTags",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateModified",
                table: "MediaFolders",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "MediaFolders",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<int>(
                name: "MediaType",
                table: "MediaFiles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "FileExtension",
                table: "MediaFiles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateModified",
                table: "MediaFiles",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "MediaFiles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_UploadedByUserId",
                table: "MediaFiles",
                column: "UploadedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaFiles_AspNetUsers_UploadedByUserId",
                table: "MediaFiles",
                column: "UploadedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaFolders_MediaFolders_ParentFolderId",
                table: "MediaFolders",
                column: "ParentFolderId",
                principalTable: "MediaFolders",
                principalColumn: "ID");
        }
    }
}
