using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FinancialManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class AddTransferAccountIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_AspNetUsers_RecipientId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_AspNetUsers_SenderId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_RecipientId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_SenderId",
                table: "Transfers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a6741bc7-4539-414d-8c29-a93a037ec1da");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d69b8a04-11db-4147-9bc6-cb154a20419c");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "Transfers");

            migrationBuilder.AddColumn<int>(
                name: "RecipientAccountId",
                table: "Transfers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SenderAccountId",
                table: "Transfers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "ab5c1d7d-c6fe-46cb-a952-2b9856b79bf9", null, "User", "USER" },
                    { "bbccfcc9-b948-41ab-b5dc-7954735639e2", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_RecipientAccountId",
                table: "Transfers",
                column: "RecipientAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_SenderAccountId",
                table: "Transfers",
                column: "SenderAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Accounts_RecipientAccountId",
                table: "Transfers",
                column: "RecipientAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Accounts_SenderAccountId",
                table: "Transfers",
                column: "SenderAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Accounts_RecipientAccountId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Accounts_SenderAccountId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_RecipientAccountId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_SenderAccountId",
                table: "Transfers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ab5c1d7d-c6fe-46cb-a952-2b9856b79bf9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bbccfcc9-b948-41ab-b5dc-7954735639e2");

            migrationBuilder.DropColumn(
                name: "RecipientAccountId",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "SenderAccountId",
                table: "Transfers");

            migrationBuilder.AddColumn<string>(
                name: "RecipientId",
                table: "Transfers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SenderId",
                table: "Transfers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a6741bc7-4539-414d-8c29-a93a037ec1da", null, "User", "USER" },
                    { "d69b8a04-11db-4147-9bc6-cb154a20419c", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_RecipientId",
                table: "Transfers",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_SenderId",
                table: "Transfers",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_AspNetUsers_RecipientId",
                table: "Transfers",
                column: "RecipientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_AspNetUsers_SenderId",
                table: "Transfers",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
