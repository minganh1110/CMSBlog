using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMSBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaFiles_AppUsers_UploadedByUserId",
                table: "MediaFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUsers",
                table: "AppUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppRoles",
                table: "AppRoles");

            migrationBuilder.RenameTable(
                name: "AppUsers",
                newName: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "AppRoles",
                newName: "AspNetRoles");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "MediaTags",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "MediaFolders",
                newName: "DateModified");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "MediaFolders",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "UploadedAt",
                table: "MediaFiles",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "MediaFiles",
                newName: "DateModified");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MediaTags",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "MediaTags",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FilePath",
                table: "MediaFiles",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "FileExtension",
                table: "MediaFiles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MediaFiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppUserTokens",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AppUserTokens",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AppUserLogins",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AppUserLogins",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedUserName",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedEmail",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedName",
                table: "AspNetRoles",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetRoles",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserRoles_UserId",
                table: "AppUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserClaims_UserId",
                table: "AppUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppRoleClaims_RoleId",
                table: "AppRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AppRoleClaims_AspNetRoles_RoleId",
                table: "AppRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserClaims_AspNetUsers_UserId",
                table: "AppUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserLogins_AspNetUsers_UserId",
                table: "AppUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserRoles_AspNetRoles_RoleId",
                table: "AppUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserRoles_AspNetUsers_UserId",
                table: "AppUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserTokens_AspNetUsers_UserId",
                table: "AppUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaFiles_AspNetUsers_UploadedByUserId",
                table: "MediaFiles",
                column: "UploadedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppRoleClaims_AspNetRoles_RoleId",
                table: "AppRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserClaims_AspNetUsers_UserId",
                table: "AppUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserLogins_AspNetUsers_UserId",
                table: "AppUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserRoles_AspNetRoles_RoleId",
                table: "AppUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserRoles_AspNetUsers_UserId",
                table: "AppUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserTokens_AspNetUsers_UserId",
                table: "AppUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaFiles_AspNetUsers_UploadedByUserId",
                table: "MediaFiles");

            migrationBuilder.DropIndex(
                name: "IX_AppUserRoles_UserId",
                table: "AppUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_AppUserClaims_UserId",
                table: "AppUserClaims");

            migrationBuilder.DropIndex(
                name: "IX_AppRoleClaims_RoleId",
                table: "AppRoleClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "EmailIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "MediaTags");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MediaFiles");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "AppUsers");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "AppRoles");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "MediaTags",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "DateModified",
                table: "MediaFolders",
                newName: "UpdateAt");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "MediaFolders",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "DateModified",
                table: "MediaFiles",
                newName: "CreateAt");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "MediaFiles",
                newName: "UploadedAt");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MediaTags",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FilePath",
                table: "MediaFiles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

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

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppUserTokens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AppUserTokens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AppUserLogins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AppUserLogins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedUserName",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedEmail",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedName",
                table: "AppRoles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppRoles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUsers",
                table: "AppUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppRoles",
                table: "AppRoles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaFiles_AppUsers_UploadedByUserId",
                table: "MediaFiles",
                column: "UploadedByUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
