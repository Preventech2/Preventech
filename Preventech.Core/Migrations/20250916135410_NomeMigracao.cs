using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Preventech.Core.Migrations
{
    /// <inheritdoc />
    public partial class NomeMigracao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Local",
                table: "Equipamentos");

            migrationBuilder.AlterColumn<string>(
                name: "Patrimonio",
                table: "Equipamentos",
                type: "character varying(120)",
                maxLength: 120,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(120)",
                oldMaxLength: 120);

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Equipamentos",
                type: "character varying(120)",
                maxLength: 120,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(120)",
                oldMaxLength: 120);

            migrationBuilder.AddColumn<int>(
                name: "LocalId",
                table: "Equipamentos",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Localizacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Campus = table.Column<int>(type: "integer", nullable: false),
                    Predio = table.Column<int>(type: "integer", nullable: false),
                    Andar = table.Column<int>(type: "integer", nullable: false),
                    Numero = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localizacao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Preditiva",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Frequencia = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ReferenteId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preditiva", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Preditiva_Equipamentos_ReferenteId",
                        column: x => x.ReferenteId,
                        principalTable: "Equipamentos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Preventiva",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Frequencia = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ArquivoReferente = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: false),
                    ReferenteId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preventiva", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Preventiva_Equipamentos_ReferenteId",
                        column: x => x.ReferenteId,
                        principalTable: "Equipamentos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Equipamentos_LocalId",
                table: "Equipamentos",
                column: "LocalId");

            migrationBuilder.CreateIndex(
                name: "IX_Preditiva_ReferenteId",
                table: "Preditiva",
                column: "ReferenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Preventiva_ReferenteId",
                table: "Preventiva",
                column: "ReferenteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipamentos_Localizacao_LocalId",
                table: "Equipamentos",
                column: "LocalId",
                principalTable: "Localizacao",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipamentos_Localizacao_LocalId",
                table: "Equipamentos");

            migrationBuilder.DropTable(
                name: "Localizacao");

            migrationBuilder.DropTable(
                name: "Preditiva");

            migrationBuilder.DropTable(
                name: "Preventiva");

            migrationBuilder.DropIndex(
                name: "IX_Equipamentos_LocalId",
                table: "Equipamentos");

            migrationBuilder.DropColumn(
                name: "LocalId",
                table: "Equipamentos");

            migrationBuilder.AlterColumn<string>(
                name: "Patrimonio",
                table: "Equipamentos",
                type: "character varying(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(120)",
                oldMaxLength: 120,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Equipamentos",
                type: "character varying(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(120)",
                oldMaxLength: 120,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Local",
                table: "Equipamentos",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
