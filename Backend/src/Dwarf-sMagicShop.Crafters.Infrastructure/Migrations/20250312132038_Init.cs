﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dwarf_sMagicShop.Infrastructure.Migrations
{
	/// <inheritdoc />
	public partial class Init : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "artefacts_species",
				columns: table => new
				{
					id = table.Column<Guid>(type: "uuid", nullable: false),
					name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("pk_artefacts_species", x => x.id);
				});

			migrationBuilder.CreateTable(
				name: "crafter_balance",
				columns: table => new
				{
					id = table.Column<Guid>(type: "uuid", nullable: false),
					crafter_id = table.Column<Guid>(type: "uuid", nullable: false),
					balance = table.Column<decimal>(type: "numeric", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("pk_crafter_balance", x => x.id);
				});

			migrationBuilder.CreateTable(
				name: "crafters",
				columns: table => new
				{
					id = table.Column<Guid>(type: "uuid", nullable: false),
					nickname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
					experience = table.Column<int>(type: "integer", nullable: false),
					socials = table.Column<string>(type: "text", nullable: true),
					deleted = table.Column<bool>(type: "boolean", nullable: false),
					deletion_date = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L)
				},
				constraints: table =>
				{
					table.PrimaryKey("pk_crafters", x => x.id);
				});

			migrationBuilder.CreateTable(
				name: "magic_artefacts",
				columns: table => new
				{
					id = table.Column<Guid>(type: "uuid", nullable: false),
					name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
					description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
					species_id = table.Column<Guid>(type: "uuid", nullable: true),
					effect = table.Column<string>(type: "text", nullable: true),
					rare = table.Column<string>(type: "text", nullable: false),
					location = table.Column<string>(type: "text", nullable: true),
					weight = table.Column<float>(type: "real", nullable: false),
					handed_amount_type = table.Column<string>(type: "text", nullable: false),
					artefact_status_type = table.Column<string>(type: "text", nullable: false),
					creation_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					image_file = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
					position = table.Column<int>(type: "integer", nullable: false),
					crafter_id = table.Column<Guid>(type: "uuid", nullable: false),
					deleted = table.Column<bool>(type: "boolean", nullable: false),
					deletion_date = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L)
				},
				constraints: table =>
				{
					table.PrimaryKey("pk_magic_artefacts", x => x.id);
					table.ForeignKey(
						name: "fk_magic_artefacts_crafters_crafter_id",
						column: x => x.crafter_id,
						principalTable: "crafters",
						principalColumn: "id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "ix_magic_artefacts_crafter_id",
				table: "magic_artefacts",
				column: "crafter_id");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "artefacts_species");

			migrationBuilder.DropTable(
				name: "crafter_balance");

			migrationBuilder.DropTable(
				name: "magic_artefacts");

			migrationBuilder.DropTable(
				name: "crafters");
		}
	}
}