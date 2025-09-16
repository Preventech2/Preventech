using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Preventech.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddedCpfCollumnForUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "cpf",
                table: "Usuarios",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cpf",
                table: "Usuarios");
        }
    }
}
