using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dwarf_sMagicShop.Accounts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AccountsAddCrafterIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "crafter_id",
                schema: "accounts",
                table: "crafter_accounts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "crafter_id",
                schema: "accounts",
                table: "crafter_accounts");
        }
    }
}
