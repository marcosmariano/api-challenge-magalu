using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LuizaLabs.Challenge.Migrations
{
    public partial class UserSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "updated_at",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2021, 3, 2, 4, 15, 30, 901, DateTimeKind.Unspecified).AddTicks(7081), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "created_at", "first_name", "last_name", "password", "role", "username" },
                values: new object[,]
                {
                    { 1, new DateTimeOffset(new DateTime(2021, 3, 2, 4, 15, 30, 921, DateTimeKind.Unspecified).AddTicks(1112), new TimeSpan(0, 0, 0, 0, 0)), "Admin", "Admin", "admin", "Admin", "admin" },
                    { 2, new DateTimeOffset(new DateTime(2021, 3, 2, 4, 15, 30, 921, DateTimeKind.Unspecified).AddTicks(2833), new TimeSpan(0, 0, 0, 0, 0)), "Normal", "User", "user", "User", "user" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "updated_at",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTimeOffset(new DateTime(2021, 3, 2, 4, 15, 30, 901, DateTimeKind.Unspecified).AddTicks(7081), new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}
