using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Preventech.Core.Migrations
{
    /// <inheritdoc />
    public partial class ChangedCpfNameForUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "cpf",
                table: "Usuarios",
                newName: "Cpf");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Cpf",
                table: "Usuarios",
                newName: "cpf");
        }
    }
}
