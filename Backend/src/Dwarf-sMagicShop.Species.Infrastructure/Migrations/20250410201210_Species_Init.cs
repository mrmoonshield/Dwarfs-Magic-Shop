using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dwarf_sMagicShop.Species.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Species_Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "species");

            migrationBuilder.CreateTable(
                name: "artefacts_species",
                schema: "species",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_artefacts_species", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "artefacts_species",
                schema: "species");
        }
    }
}
