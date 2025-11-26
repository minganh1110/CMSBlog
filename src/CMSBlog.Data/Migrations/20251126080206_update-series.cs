using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMSBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateseries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Series",
                table: "Series");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostInSeries",
                table: "PostInSeries");

            migrationBuilder.DropIndex(
                name: "IX_PostInSeries_Slug",
                table: "PostInSeries");

            migrationBuilder.DropColumn(
                name: "PaidDate",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "PostInSeries");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "PostInSeries");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "PostInSeries");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "PostInSeries");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "PostInSeries");

            migrationBuilder.DropColumn(
                name: "SeoDescription",
                table: "PostInSeries");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "PostInSeries");

            migrationBuilder.RenameColumn(
                name: "TagId",
                table: "Series",
                newName: "AuthorUserId");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "Series",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "SortOrder",
                table: "PostInSeries",
                newName: "DisplayOrder");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PostInSeries",
                newName: "SeriesId");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Series",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Series",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Series",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Series",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Series",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeoDescription",
                table: "Series",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Series",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "Series",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "Series",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PostId",
                table: "PostInSeries",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Series",
                table: "Series",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostInSeries",
                table: "PostInSeries",
                columns: new[] { "PostId", "SeriesId" });

            migrationBuilder.CreateIndex(
                name: "IX_Series_Slug",
                table: "Series",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Series",
                table: "Series");

            migrationBuilder.DropIndex(
                name: "IX_Series_Slug",
                table: "Series");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostInSeries",
                table: "PostInSeries");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "SeoDescription",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "PostInSeries");

            migrationBuilder.RenameColumn(
                name: "AuthorUserId",
                table: "Series",
                newName: "TagId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Series",
                newName: "PostId");

            migrationBuilder.RenameColumn(
                name: "DisplayOrder",
                table: "PostInSeries",
                newName: "SortOrder");

            migrationBuilder.RenameColumn(
                name: "SeriesId",
                table: "PostInSeries",
                newName: "Id");

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidDate",
                table: "Posts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "PostInSeries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "PostInSeries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "PostInSeries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PostInSeries",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "PostInSeries",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoDescription",
                table: "PostInSeries",
                type: "nvarchar(160)",
                maxLength: 160,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "PostInSeries",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Series",
                table: "Series",
                columns: new[] { "PostId", "TagId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostInSeries",
                table: "PostInSeries",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PostInSeries_Slug",
                table: "PostInSeries",
                column: "Slug",
                unique: true);
        }
    }
}
