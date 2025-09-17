using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Preventech.Core.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoRequisitanteOS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequisitanteId",
                table: "OrdensServico",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrdensServico_RequisitanteId",
                table: "OrdensServico",
                column: "RequisitanteId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdensServico_Usuarios_RequisitanteId",
                table: "OrdensServico",
                column: "RequisitanteId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdensServico_Usuarios_RequisitanteId",
                table: "OrdensServico");

            migrationBuilder.DropIndex(
                name: "IX_OrdensServico_RequisitanteId",
                table: "OrdensServico");

            migrationBuilder.DropColumn(
                name: "RequisitanteId",
                table: "OrdensServico");
        }
    }
}
