using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace trial.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOwnerFromPermissionPolicy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissionPolicies_Users_GuestId",
                table: "PermissionPolicies");

            migrationBuilder.DropForeignKey(
                name: "FK_PermissionPolicies_Users_OwnerId",
                table: "PermissionPolicies");

            migrationBuilder.DropIndex(
                name: "IX_PermissionPolicies_GuestId",
                table: "PermissionPolicies");

            migrationBuilder.DropIndex(
                name: "IX_PermissionPolicies_OwnerId_GuestId_NoteId",
                table: "PermissionPolicies");

            migrationBuilder.DropColumn(
                name: "GuestId",
                table: "PermissionPolicies");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "PermissionPolicies",
                newName: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionPolicies_UserId_NoteId",
                table: "PermissionPolicies",
                columns: new[] { "UserId", "NoteId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionPolicies_Users_UserId",
                table: "PermissionPolicies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissionPolicies_Users_UserId",
                table: "PermissionPolicies");

            migrationBuilder.DropIndex(
                name: "IX_PermissionPolicies_UserId_NoteId",
                table: "PermissionPolicies");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "PermissionPolicies",
                newName: "OwnerId");

            migrationBuilder.AddColumn<Guid>(
                name: "GuestId",
                table: "PermissionPolicies",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PermissionPolicies_GuestId",
                table: "PermissionPolicies",
                column: "GuestId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionPolicies_OwnerId_GuestId_NoteId",
                table: "PermissionPolicies",
                columns: new[] { "OwnerId", "GuestId", "NoteId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionPolicies_Users_GuestId",
                table: "PermissionPolicies",
                column: "GuestId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionPolicies_Users_OwnerId",
                table: "PermissionPolicies",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
