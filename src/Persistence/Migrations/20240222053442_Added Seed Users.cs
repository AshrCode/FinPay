using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedSeedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "IsVerified" },
                values: new object[,]
                {
                    { new Guid("6751304e-0eea-443c-ad6a-dfbbf53731fe"), false },
                    { new Guid("b136cf3d-766b-45ae-aa84-ac7f10c5a090"), true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("6751304e-0eea-443c-ad6a-dfbbf53731fe"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b136cf3d-766b-45ae-aa84-ac7f10c5a090"));
        }
    }
}
