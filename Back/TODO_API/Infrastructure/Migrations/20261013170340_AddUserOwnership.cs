using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserOwnership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "TaskItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Categories",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.Sql("UPDATE Categories SET UserId = (SELECT Id FROM Users WHERE CreatedAt <= Categories.CreatedAt LIMIT 1) WHERE (SELECT COUNT(*) FROM Users WHERE CreatedAt <= Categories.CreatedAt) = 1");
            migrationBuilder.Sql("UPDATE TaskItems SET UserId = (SELECT Id FROM Users WHERE CreatedAt <= TaskItems.CreatedAt LIMIT 1) WHERE (SELECT COUNT(*) FROM Users WHERE CreatedAt <= TaskItems.CreatedAt) = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Categories");
        }
    }
}
